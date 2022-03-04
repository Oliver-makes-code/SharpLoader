namespace SharpLoader;

using System.Reflection;

public class Loader {
    public string directory {get; private set;}
    public string extension {get; private set;}
    public Loader(string modDir, string fileExt = "dll") {
        directory = modDir;
        extension = fileExt;
    }

    public Mod[] LoadMods() {
        List<Mod> mods = new List<Mod>();
        string[] files = Directory.GetFiles(directory);

        foreach (string file in files) {
            if (file.EndsWith("."+extension)) {
                var modFile = Assembly.LoadFile(file);
                
                foreach (Type type in modFile.GetExportedTypes()) {
                    if (type.IsAssignableFrom(typeof(ModInterface))) {
                        ModInterface instance = (ModInterface) Activator.CreateInstance(type);
                        instance.Load();
                        Mod mod = new Mod(instance);
                        mods.Add(mod);
                    }
                }
                
            }
        }

        return mods.ToArray<Mod>();
    }
}

public class Mod {
    public ModInterface instance {get; private set;}
    public Mod(ModInterface modInstance) {
        instance = modInstance;
    }
}

public interface ModInterface {
    public void Load();
    public void UnLoad();

    //Add methods to this for your needs
}