using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WizardsoftTest.Interfaces;
using WizardsoftTest.Models;
using WizardsoftTest.Settings;

namespace WizardsoftTest.Services
{
    public class FolderService : IFolderService
    {
        private readonly IMongoCollection<Folder> _foldersCollection;

        public FolderService(
        IOptions<FoldersDatabaseSettings> foldersDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                foldersDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                foldersDatabaseSettings.Value.DatabaseName);

            _foldersCollection = mongoDatabase.GetCollection<Folder>(
                foldersDatabaseSettings.Value.FoldersCollectionName);
        }

        public async Task<List<Folder>> GetAsync() =>
            await _foldersCollection.Find(_ => true).ToListAsync();

        public async Task<Folder?> GetAsync(string id) =>
            await _foldersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Folder newFolder) =>
            await _foldersCollection.InsertOneAsync(newFolder);

        public async Task UpdateAsync(string id, Folder updatedFolder) =>
            await _foldersCollection.ReplaceOneAsync(x => x.Id == id, updatedFolder);

        public async Task RemoveAsync(string id) =>
            await _foldersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
