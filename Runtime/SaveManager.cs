using System.Threading.Tasks;
using Persistence;

namespace SaveManagement
{
  public class SaveData { }

  public interface ISaveable<T> where T : SaveData
  {
    public string FileName { get; }
    public void Save(T data);
    public void Load(T data);
  }

  public static class SaveManager
  {
    public static async Task SaveSaveable<T, U>(T saveable, string fileName = null) where T : ISaveable<U> where U : SaveData, new()
    {
      U data = new U();
      saveable.Save(data);
      await JsonPersistence.PersistJson(data, fileName != null ? fileName : saveable.FileName);
    }

    public static async Task LoadSaveable<T, U>(T saveable, string fileName = null) where T : ISaveable<U> where U : SaveData
    {
      U data = await JsonPersistence.FromJson<U>(fileName != null ? fileName : saveable.FileName);
      saveable.Load(data);
    }

    public static bool SaveableExists<T, U>(T saveable) where T : ISaveable<U> where U : SaveData
    {
      return JsonPersistence.JsonExists(saveable.FileName);
    }
  }
}