using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

namespace ModMuseum
{
    public class ModMuseum : ModBehaviour
    {
        public static ModMuseum Instance;
        public INewHorizons NewHorizons;

        // custom variables
        private bool hasWokeUp;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(ModMuseum)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);

            new Harmony("Samster68OW.ModMuseum").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }
        
        // funny death when player talks to scientist
        public void Update()
        {
            if (DialogueConditionManager.SharedInstance.GetConditionState("THETRUTH_REVEALED") && !hasWokeUp)
            {
                Locator.GetDeathManager().KillPlayer(DeathType.Energy); // kills player
                hasWokeUp = true; // sets bool to true so it doesn't run again
            }
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);

            // sets this to false at the start of the loop
            hasWokeUp = false;
        }
    }

}
