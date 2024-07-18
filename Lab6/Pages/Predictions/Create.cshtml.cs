using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Storage.Blobs;
using Azure;
using Lab6.Models.Prediction;

namespace Lab6.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly Lab6.Data.PredictionDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        public CreateModel(Lab6.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(ICollection<IFormFile> files)
        {
            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(earthContainerName);
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(earthContainerName);
            }

            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(computerContainerName);
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(computerContainerName);
            }

            foreach (var file in files)
            {
                try
                {
                    // create the blob to hold the data
                    var blob = containerClient.GetBlobClient(file.FileName);
                    if (await blob.ExistsAsync())
                    {
                        await blob.DeleteAsync();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        // copy the file data into memory
                        await file.CopyToAsync(memoryStream);

                        //This will navigate back to the beginning of the memory which is memory position 0
                        memoryStream.Position = 0;

                        // send the file to the cloud
                        await blob.UploadAsync(memoryStream);
                        memoryStream.Close();

                    }

                }
                catch (RequestFailedException)
                {
                    return RedirectToPage("./Error");
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
