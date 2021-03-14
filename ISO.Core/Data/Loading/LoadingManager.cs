using ISO.Core.Data.DataLoader;
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
    public class LoadingManager
    {

        public LoadingManager(ISOContentManager manager)
        {
            this.manager = manager;

            this.Textures = new Dictionary<string, TextureAsset>();
            this.Sounds = new Dictionary<string, SoundAsset>();
            this.Fonts = new Dictionary<string, FontAsset>();
            this.DataCallback = new Dictionary<string, Action>();

            AssetsCount = 0;
            AssetsLoaded = 0;
        }

        private ISOContentManager manager { get; set; }

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
        public Action<LoadingManager> AfterLoadCallback { get; set; }

        /// <summary>
        /// Flag when game is asynchronously loading data
        /// </summary>
        public bool IsLoading { get; set; }

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
        public TextureAsset GetTexture(string name) => Textures[name];

        /// <summary>
        /// Returns loaded sound
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SoundAsset GetSound(string name) => Sounds[name];

        /// <summary>
        /// returns loaded fonts
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FontAsset GetFont(string name) => Fonts[name];

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
                Textures.TryAdd(name, new TextureAsset(path));
                AssetsCount++;
            }
            else if (tType.Equals(typeof(SoundAsset)))
            {
                Sounds.TryAdd(name, new SoundAsset(path));
                AssetsCount++;
            }
            else if (tType.Equals(typeof(FontAsset)))
            {
                Fonts.TryAdd(name, new FontAsset(path));
                AssetsCount++;
            }
            else if (tType.Equals(typeof(DataAsset)))
            {
                Log.Error("Use PrepareDataTable() for DataAsset!");
            }
            else
            {
                Log.Error("Unknow type, asset " + name + "was not loaded!");
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
                Log.Error("Use UnloadCallback for DataAsset!");
            }
            else
            {
                Log.Error("Unknow type, asset " + name + "was not loaded!");
            }
        }

        /// <summary>
        /// Starts Async data loading (warning - any update or draw method with reference to null will cause error)
        /// </summary>
        public void StartLoadingAsync()
        {
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
            IsLoading = true;

            List<string> keysToRemove = Textures.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Textures
            {
                var textureAsset = Textures[key];

                textureAsset.Texture.Dispose();
                textureAsset.Texture = null;

                Textures.Remove(key);

                AssetsCount--;
                AssetsLoaded--;
            }

            keysToRemove = Sounds.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Sounds
            {
                var soundAsset = Sounds[key];

                soundAsset.Sound.Dispose();
                soundAsset.Sound = null;

                Sounds.Remove(key);

                AssetsCount--;
                AssetsLoaded--;
            }

            keysToRemove = Fonts.Where(x => x.Value.Unload == true).Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Sounds
            {
                var fontAsset = Fonts[key];

                fontAsset.Font = null;

                Fonts.Remove(key);

                AssetsCount--;
                AssetsLoaded--;
            }

            keysToRemove = DataCallback.Select(x => x.Key).ToList();

            foreach (var key in keysToRemove) // Callbacks
            {
                DataCallback.Remove(key);

                AssetsCount--;
                AssetsLoaded--;
            }

            IsLoading = false;
        }


        /// <summary>
        /// Method for loading data
        /// </summary>
        private void LoadAssets()
        {
            IsLoading = true;

            foreach (var callback in DataCallback)
            {
                callback.Value.Invoke();
            }
            LoadingCallBackInvoked = true;

            foreach (var texture in Textures)
            {
                texture.Value.Texture = manager.Load<Texture2D>(texture.Value.Path);
                AssetsLoaded++;
            }

            foreach (var sound in Sounds)
            {
                sound.Value.Sound = manager.Load<SoundEffect>(sound.Value.Path);
                AssetsLoaded++;
            }

            foreach (var font in Fonts)
            {
                font.Value.Font = manager.Load<SpriteFont>(font.Value.Path);
                AssetsLoaded++;
            }


            //Log.Warning("Sleeping for 5000ms");
            //Thread.Sleep(5000);

            AfterLoadCallback.Invoke(this);
            IsLoading = false;
            LoadingCallBackInvoked = false;
        }
    }
}
