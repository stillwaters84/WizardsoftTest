using WizardsoftTest;
using WizardsoftTest.Controllers;
using WizardsoftTestApp.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

Console.WriteLine("This is a console app without inputs.");

    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:5001");

WizardsoftApiWork(client).Wait();

static async Task WizardsoftApiWork(HttpClient client)
{
    try
    {
        using (client)
        {
            //GET ALL
            Console.WriteLine("Firstly, it will get all elements collection from DB");
            List<Folder> collection = new List<Folder>();
            collection = await GetCollection(client);
            if (collection is not null)
            {
                Console.WriteLine("\nInitial Collection:\n");
                foreach (var elem in collection)
                {
                    Console.WriteLine($"Id: {elem.Id}\nName: {elem.Name}\nParentId: {elem.FolderParentId}\n");
                }
            }

            //POST NEW
            Console.WriteLine("Secondly, it will create new element and post it in collection");
            Console.WriteLine("Note: we can't create new folder without parent because it will ruin main heirarchy");

            Folder newFolder = new Folder { Name = "ConsoleFolder1", FolderParentId = collection[1].Id }; //my db always have at least 3 elements in collection so i take second id as first branch
            JsonContent content = JsonContent.Create(newFolder);
            var newFolderReturned = await PostFolder(client, content);
            if (newFolderReturned is not null)
            {
                Console.WriteLine($"\nNew folder: \nId: {newFolderReturned.Id}\nName: {newFolderReturned.Name}\nParentId: {newFolderReturned.FolderParentId}\n");
            }

            //GET ALL UPDATED
            collection = await GetCollection(client);
            if (collection is not null)
            {
                Console.WriteLine("\nnUpdated Collection:\n");
                foreach (var elem in collection)
                {
                    Console.WriteLine($"Id: {elem.Id}\nName: {elem.Name}\nParentId: {elem.FolderParentId}\n");
                }
            }

            //PUT NEW
            Console.WriteLine("Thirdly, it will change name of new folder and make it a child of second branch folder");
            Console.WriteLine("Note: if name or parent id of folder will be empty or null it will take old value");

            newFolderReturned.Name = "ConsoleFolderUpdated2";
            newFolderReturned.FolderParentId = collection[2].Id;
            content = JsonContent.Create(newFolderReturned);
            var updatedFolderReturned = await UpdateFolder(client, newFolderReturned.Id, content);
            if (updatedFolderReturned is not null)
            {
                Console.WriteLine($"\nNew folder: \nId: {updatedFolderReturned.Id}\nName: {updatedFolderReturned.Name}\nParentId: {updatedFolderReturned.FolderParentId}\n");
            }

            //GET ALL UPDATED
            collection = await GetCollection(client);
            if (collection is not null)
            {
                Console.WriteLine("\nnUpdated Collection:\n");
                foreach (var elem in collection)
                {
                    Console.WriteLine($"Id: {elem.Id}\nName: {elem.Name}\nParentId: {elem.FolderParentId}\n");
                }
            }

            //DELETE NEW
            Console.WriteLine("Lastly, we will delete second folder from collection");
            Console.WriteLine("Note: if we want to delete folder with parent and childs, we will simply change parent ids of child");
            var secondFolder = collection[2];
            var childFolder = collection.Find(x => x.FolderParentId == secondFolder.Id);
            if(childFolder is not null)
            {
                childFolder.FolderParentId = secondFolder.FolderParentId;
                content = JsonContent.Create(childFolder);
                var newUpdated = await UpdateFolder(client, childFolder.Id, content);
                var result = await DeleteFolder(client, secondFolder.Id);
                Console.WriteLine($"Result of deletion: {result}");
            }

            //GET ALL DELETED
            collection = await GetCollection(client);
            if (collection is not null)
            {
                Console.WriteLine("\nnUpdated Collection:\n");
                foreach (var elem in collection)
                {
                    Console.WriteLine($"Id: {elem.Id}\nName: {elem.Name}\nParentId: {elem.FolderParentId}\n");
                }
            }

            Console.WriteLine("\nThat's all for now");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static async Task<List<Folder>> GetCollection(HttpClient client)
{
    HttpResponseMessage result = await client.GetAsync("/api/Folders");
    if (result.IsSuccessStatusCode)
    {
        var resultString = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Folder>>(resultString);
    }
    return null;
}

static async Task<Folder> PostFolder(HttpClient client, JsonContent newFolder)
{
    HttpResponseMessage result = await client.PostAsync("/api/Folders", newFolder);
    if (result.IsSuccessStatusCode)
    {
        var resultString = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Folder>(resultString);
    }
    return null;
}

static async Task<Folder> UpdateFolder(HttpClient client, string folderId, JsonContent updatingFolder)
{
    HttpResponseMessage result = await client.PutAsync($"/api/Folders/{folderId}", updatingFolder);
    if (result.IsSuccessStatusCode)
    {
        var resultString = await result.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Folder>(resultString);
    }
    return null;
}

//Need to also delete all childs and more OR change parent id's
static async Task<string> DeleteFolder(HttpClient client, string folderId)
{
    HttpResponseMessage result = await client.DeleteAsync($"/api/Folders/{folderId}");
    if (result.IsSuccessStatusCode)
    {
        var resultString = await result.Content.ReadAsStringAsync();
        return resultString;
    }
    return null;
}