using WizardsoftTest.Models;

namespace WizardsoftTest.Interfaces
{
    public interface IFolderService
    {
        Task<List<Folder>> GetAsync();

        Task<Folder?> GetAsync(string id);

        Task CreateAsync(Folder newFolder);

        Task UpdateAsync(string id, Folder updatedFolder);

        Task RemoveAsync(string id);
    }
}
