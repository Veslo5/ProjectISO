using ISO.Core.Data.DataLoader;
using ISO.Core.Engine.Helpers.Extensions.ErrorHandling;
using ISO.Core.Engine.Logging;
using ISO.Core.Loading.Assets;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ISO.Core.Loading
{
    public class LoadingController
    {

        public LoadingController(ISOContentProvider manager)
        {
            this.manager = manager;

            this.Textures = new Dictionary<string, TextureAsset>();
            this.Sounds = new Dictionary<string, SoundAsset>();
            this.Fonts = new Dictionary<string, FontAsset>();
            this.DataCallback = new Dictionary<string, Action>();

            AssetsCount = 0;
            AssetsLoaded = 0;
        }

        private ISOContentProvider manager { get; set; }

        /// <summary>
        /// Container with textures
        /// </summary>
        private Dictionary<string, TextureAsset> Textures { get; set; }

        /// <summary>
        /// Container with Sounds
        /// </summary>
        private Dictionary<string, SoundAsset> Sounds { get; set; }

        /// <summary>
        /// Container with Fonts
        /// </summary>
        private Dictionary<string, FontAsset> Fonts { get; set; }

        /// <summary>
        /// Container with loading callbacks
        /// </summary>
        private Dictionary<string, Action> DataCallback { get; set; }

        /// <summary>
        /// Callback when data is loaded
        /// </summary>
        public Action<LoadingController> AfterLoadCallback { get; set; }

        /// <summary>
        /// Flag when game is asynchronously loading data
        /// </summary>
        public bool IsLoading { get; set; } = true;

        /// <summary>
        /// When loading data is in progress cannot add more
        /// </summary>
        public bool LoadingCallBackInvoked { get; set; } = false;

        /// <summary>
        /// Assets count to be loaded
        /// </summary>
        public int AssetsCount { get; set; }

        /// <summary>
        /// Current assets in memory
        /// </summary>
        public int AssetsLoaded { get; set; }


        /// <summary>
        /// Returns loaded texture
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TextureAsset GetTexture(string name)
        {
            TextureAsset value = null;

            if (Textures.TryGetValue(name, out value))
            {
                return value;
            }
            else
            {
                Log.Error("Texture asset " + name + "not found!");
                return null;
            }

        }

        /// <summary>
        /// Returns loaded sound
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoundAsset GetSound(string name)
        {
            SoundAsset value = null;

            if (Sounds.TryGetValue(name, out value))
            {
                return value;
            }
            else
            {
                Log.Error("Sound asset " + name + "not found!");
                return null;
            }
        }

        /// <summary>
        /// returns loaded fonts
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FontAsset GetFont(string name)
        {
            FontAsset value = null;

            if (Fonts.TryGetValue(name, out value))
            {
                return value;
            }
            else
            {
                Log.Error("Font asset " + name + "not found!");
                return null;
            }
        }

        /// <summary>
        /// Load loading callback
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callback"></param>
        public void LoadCallback(string name, Action callback)
        {
            if (LoadingCallBackInvoked == true)
            {
                throw new Exception("Cannot add more data into loading queue when loading is in progress.");
            }

            DataCallback.TryAdd(name, callback);
            AssetsCount++;
        }

        /// <summary>
        /// Adds any Asset to loading queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public void Load<T>(string name, string path) where T : AssetBase
        {
            if (LoadingCallBackInvoked == true)
            {
                throw new Exception("Cannot add more data into loading queue when loading is in progress.");
            }


            var tType = typeof(T);
            if (tType.Equals(typeof(TextureAsset)))
            {
                if (Textures.TryAdd(name, new TextureAsset(path)))
                {
                    AssetsCount++;
                    Log.Info("Added texture asset " + name + " to loading queue", LogModule.LO);
                }
            }
            else if (tType.Equals(typeof(SoundAsset)))
            {
                if (Sounds.TryAdd(name, new SoundAsset(path)))
                {
                    AssetsCount++;
                    Log.Info("Added sound asset " + name + " to loading queue", LogModule.LO);
                }
            }
            else if (tType.Equals(typeof(FontAsset)))
            {
                if (Fonts.TryAdd(name, new FontAsset(path)))
                {
                    AssetsCount++;
                    Log.Info("Added font asset " + name + " to loading queue", LogModule.LO);
                }
            }
            else if (tType.Equals(typeof(DataAsset)))
            {
                Log.Error("Use PrepareDataTable() for DataAsset!", LogModule.LO);
            }
            else
            {
                Log.Error("Unknow type, asset " + name + "was not loaded!", LogModule.LO);
            }
        }

        /// <summary>
        /// Flags assets as ready to be unloaded
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        public void Unload<T>(string name) where T : AssetBase
        {
            var tType = typeof(T);
            if (tType.Equals(typeof(TextureAsset)))
            {
                Textures[name].Unload = true;
            }
            else if (tType.Equals(typeof(SoundAsset)))
            {
                Sounds[name].Unload = true;
            }
            else if (tType.Equals(typeof(FontAsset)))
            {
                Fonts[name].Unload = true;
            }
            else if (tType.Equals(typeof(DataAsset)))
            {
                Log.Error("Use UnloadCallback for DataAsset!", LogModule.LO);
            }
            else
            {
                Log.Error("Unknow type, asset " + name + "was not loaded!", LogModule.LO);
            }
        }

        /// <summary>
        /// Starts Async data loading (warning - any update or draw method with reference to null will cause error)
        /// </summary>
        public void StartLoadingAsync()
        {
            Log.Info("Loading assets from loading queue asynchronously.", LogModule.LO);

            Task.Run(() =>
            {
                LoadAssets();
            });
        }

        /// <summary>
        /// Start data loading (will freeze the window)
        /// </summary>
        public void StartLoading()
        {
            Log.Info("Loading assets from loading queue asynchronously.", LogModule.LO);

            LoadAssets();
        }

        /// <summary>
        /// Starts unloading data async
        /// </summary>
        public void StartUnloadingAsync()
        {
            Task.Run(() =>
            {
                UnloadAssets();
            });
        }

        /// <summary>
        /// Stars unloading data (will freeze the window)
        /// </summary>
        public void StartUnloading()
        {
            UnloadAssets();
        }

        /// <summary>
        /// Method for unloading data
        /// </summary>
        private void UnloadAssets()
        {
            Log.Info("---------------------------UNLOADING---------------------------", LogModule.LO);

            IsLoading = true;

            List<string> keysToRemove = Textures.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Textures
            {
                var textureAsset = Textures[key];

                Log.Info("Unloading texture asset " + key, LogModule.LO);

                if (textureAsset.Texture != null)
                {
                    if (!textureAsset.IsResourceCached)
                    {
                        textureAsset.Texture.Dispose();
                        textureAsset.Texture = null;

                        AssetsLoaded--;
                    }
                    else
                    {
                        Log.Warning("Texture asset " + key + " is using cached reference, continuing", LogModule.LO);
                    }
                }

                AssetsCount--;
                Textures.Remove(key);
                manager.RemoveCachedAsset(textureAsset.Path);

            }

            keysToRemove = Sounds.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Sounds
            {
                var soundAsset = Sounds[key];

                Log.Info("Unloading sound asset " + key, LogModule.LO);

                if (soundAsset.Sound != null)
                {
                    if (!soundAsset.IsResourceCached)
                    {
                        soundAsset.Sound.Dispose();
                        soundAsset.Sound = null;

                        AssetsLoaded--;
                    }
                    else
                    {
                        Log.Warning("Sound asset " + key + " is using cached reference, continuing", LogModule.LO);
                    }
                }

                AssetsCount--;
                Sounds.Remove(key);
                manager.RemoveCachedAsset(soundAsset.Path);

            }

            keysToRemove = Fonts.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Sounds
            {
                var fontAsset = Fonts[key];

                Log.Info("Unloading font asset " + key, LogModule.LO);

                if (fontAsset.Font != null)
                {
                    if (!fontAsset.IsResourceCached)
                    {
                        fontAsset.Font = null;

                        AssetsLoaded--;
                    }
                    else
                    {
                        Log.Warning("Font asset " + key + " is using cached reference, continuing", LogModule.LO);
                    }
                }

                AssetsCount--;
                Fonts.Remove(key);
                manager.RemoveCachedAsset(fontAsset.Path);

            }

            keysToRemove = DataCallback.Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Callbacks
            {
                DataCallback.Remove(key);

                Log.Info("Unloading callback asset " + key, LogModule.LO);

                AssetsCount--;
            }

            Log.Info("Unloading succeeded with current " + AssetsLoaded + " assets loaded", LogModule.LO);
            Log.Info("Current amount of assets: " + AssetsCount, LogModule.LO);

            IsLoading = false;
        }


        /// <summary>
        /// Method for loading data
        /// </summary>
        private void LoadAssets()
        {
            Log.Info("---------------------------LOADING---------------------------", LogModule.LO);

            IsLoading = true;

            foreach (var callback in DataCallback)
            {
                callback.Value.Invoke();
            }

            LoadingCallBackInvoked = true;

            foreach (var texture in Textures)
            {
                loadTexture(texture);
            }

            foreach (var sound in Sounds)
            {
                loadSound(sound);
            }

            foreach (var font in Fonts)
            {
                loadFont(font);
            }

            Log.Info("Queue loading finished succesfully with " + AssetsLoaded + " assets loaded into memory", LogModule.LO);


            //Log.Warning("Loading paused for 5000ms for the sake of development!", LogModule.LO);
            //Thread.Sleep(5000);


            AfterLoadCallback.Invoke(this);
            IsLoading = false;
            LoadingCallBackInvoked = false;
        }

        public void MarkAllAsUnload()
        {
            Log.Warning("Marking all resources to unload! This action will erase everything from memory!", LogModule.LO);

            foreach (var asset in Textures)
            {
                asset.Value.Unload = true;
            }

            foreach (var asset in Sounds)
            {
                asset.Value.Unload = true;
            }

            foreach (var asset in Fonts)
            {
                asset.Value.Unload = true;
            }
        }

        private void loadTexture(KeyValuePair<string, TextureAsset> textureAsset)
        {
            try
            {
                if (!manager.IsAssetCached(textureAsset.Value.Path))
                    AssetsLoaded++;
                else
                {
                    textureAsset.Value.IsResourceCached = true;
                    Log.Warning("Texture asset " + textureAsset.Key + " is already cached, skipping loading", LogModule.LO);
                }

                // if asset is already loaded, next loading with same Path value will return same once loaded object
                textureAsset.Value.Texture = manager.Load<Texture2D>(textureAsset.Value.Path);

            }
            catch (Exception ex)
            {
                Log.Error("Could not load texture resource for Asset " + textureAsset.Key + " at path " + textureAsset.Value.Path, LogModule.LO);
                Log.Error(ex.GetAllInnerExceptions(), LogModule.LO);

                AssetsLoaded--; // rollback
            }
        }

        private void loadSound(KeyValuePair<string, SoundAsset> soundAsset)
        {
            try
            {
                if (!manager.IsAssetCached(soundAsset.Value.Path))
                    AssetsLoaded++;
                else
                {
                    soundAsset.Value.IsResourceCached = true;
                    Log.Warning("Sound asset " + soundAsset.Key + " is already cached, skipping loading", LogModule.LO);
                }

                // if asset is already loaded, next loading with same Path value will return same once loaded object
                soundAsset.Value.Sound = manager.Load<SoundEffect>(soundAsset.Value.Path);

            }
            catch (Exception ex)
            {
                Log.Error("Could not load sound resource for Asset " + soundAsset.Key + " at path " + soundAsset.Value.Path, LogModule.LO);
                Log.Error(ex.GetAllInnerExceptions(), LogModule.LO);

                AssetsLoaded--; // rollback
            }
        }

        private void loadFont(KeyValuePair<string, FontAsset> fontAsset)
        {
            try
            {
                if (!manager.IsAssetCached(fontAsset.Value.Path)) // Check if asset is already loaded
                    AssetsLoaded++;
                else
                {
                    fontAsset.Value.IsResourceCached = true;
                    Log.Warning("Font asset " + fontAsset.Key + " is already cached, skipping loading", LogModule.LO);
                }

                // if asset is already loaded, next loading with same Path value will return same once loaded object
                fontAsset.Value.Font = manager.Load<SpriteFont>(fontAsset.Value.Path);

            }
            catch (Exception ex)
            {
                Log.Error("Could not load font resource for Asset " + fontAsset.Key + " at path " + fontAsset.Value.Path, LogModule.LO);
                Log.Error(ex.GetAllInnerExceptions(), LogModule.LO);

                AssetsLoaded--; // rollback
            }
        }



    }
}
