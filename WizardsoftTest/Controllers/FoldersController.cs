using Microsoft.AspNetCore.Mvc;
using WizardsoftTest.Interfaces;
using WizardsoftTest.Models;
using WizardsoftTest.Services;

namespace WizardsoftTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;

        public FoldersController(FolderService folderService) =>
            _folderService = folderService;

        [HttpGet]
        public async Task<List<Folder>> Get() =>
            await _folderService.GetAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Folder>> Get(string id)
        {
            var folder = await _folderService.GetAsync(id);

            if (folder is null)
            {
                return NotFound();
            }

            return folder;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Folder newFolder)
        {
            if (string.IsNullOrEmpty(newFolder.FolderName) || newFolder.FolderParentId is null)
            {
                return BadRequest();
            }

            if (newFolder.FolderParentId is not null && await _folderService.GetAsync(newFolder.FolderParentId) is null)
            {
                return NotFound();
            }

            await _folderService.CreateAsync(newFolder);

            return CreatedAtAction(nameof(Get), new { id = newFolder.Id }, newFolder);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Folder updatedFolder)
        {
            var folder = await _folderService.GetAsync(id);

            if (folder is null)
            {
                return NotFound();
            }

            updatedFolder.Id = folder.Id;

            if(updatedFolder.FolderParentId is null)
            {
                updatedFolder.FolderParentId = folder.Id;
            }

            if(updatedFolder.FolderName is null)
            {
                updatedFolder.FolderName = folder.FolderName;
            }

            await _folderService.UpdateAsync(id, updatedFolder);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var folder = await _folderService.GetAsync(id);

            if (folder is null)
            {
                return NotFound();
            }

            await _folderService.RemoveAsync(id);

            return Ok();
        }
    }
}
