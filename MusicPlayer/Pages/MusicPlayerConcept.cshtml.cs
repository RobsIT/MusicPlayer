using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class MusicPlayerConcept : PageModel
{
    [BindProperty]
    public List<IFormFile> AudioFiles { get; set; }

    public List<string> AudioFilesList { get; set; } = new List<string>();

    public void OnGet()
    {
        // Hämta alla ljudfiler från wwwroot/audio
        var audioDirectory = Path.Combine("wwwroot/audio");
        if (Directory.Exists(audioDirectory))
        {
            AudioFilesList = Directory.GetFiles(audioDirectory)
                                      .Select(file => "/audio/" + Path.GetFileName(file))
                                      .ToList();
        }
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (AudioFiles != null && AudioFiles.Any())
        {
            foreach (var audioFile in AudioFiles)
            {
                var filePath = Path.Combine("wwwroot/audio", audioFile.FileName);

                // Kontrollera om filen redan finns för att undvika duplicering
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await audioFile.CopyToAsync(stream);
                    }
                }
            }
        }

        return RedirectToPage();
    }

}