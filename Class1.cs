using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Native;
using System.Windows.Forms;
using System.Threading;

namespace Back_To_The_Future
{
    public class Class1 : Script
    {
        static int ticker = 0;
        static int hour = 22;
        static int minute = 0;
        static int enabled = 1;
        public Class1()
        {
            Tick += onTick;
            KeyDown += onKeyDown;
        }

        void onTick(object sender, EventArgs e)
        {

            Ped player = Game.Player.Character;
            int h = GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CLOCK_HOURS);
            int m = GTA.Native.Function.Call<int>(GTA.Native.Hash.GET_CLOCK_MINUTES);
            if (player.IsInVehicle())
            {
                Vehicle veh = player.CurrentVehicle;

                //  If only the Space Docker had a license plate :(
                //  GTA.Native.Function.Call(GTA.Native.Hash.SET_VEHICLE_NUMBER_PLATE_TEXT, veh, "OUTATIME");
                if (veh.DisplayName.ToLower() == "dune2" && enabled == 1 && !veh.IsInAir)
                {
                    if (veh.Speed >= 31)
                    {
                        while (veh.Speed > 10)
                            veh.Speed--;
                        if (h + 12 > 23)
                            h = h - 23;

                        //Start Ped Despawning
                        Ped[] peds = World.GetNearbyPeds(player, 1000);
                        Vehicle[] pedVehicles = World.GetNearbyVehicles(player, 1000);
                        for (int i = 0; i < peds.Length; i++)
                        {
                            if (peds[i] != player)
                                GTA.Native.Function.Call(GTA.Native.Hash.SET_ENTITY_COORDS_NO_OFFSET, peds[i], 0, 0, 0, 0, 0, 1);

                        }
                        Array.Clear(peds, 0, peds.Length);
                        for (int i = 0; i < pedVehicles.Length; i++)
                        {
                            if (pedVehicles[i] != player.CurrentVehicle)
                                GTA.Native.Function.Call(GTA.Native.Hash.SET_ENTITY_COORDS_NO_OFFSET, pedVehicles[i], 0, 0, 0, 0, 0, 1);
                        }
                        Array.Clear(pedVehicles, 0, pedVehicles.Length);
                        //End Ped Despawning

                        GTA.Native.Function.Call(GTA.Native.Hash.ADD_EXPLOSION, player.Position.X, player.Position.Y + 1, player.Position.Z, 4, 1, true, true, 20);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_CLOCK_TIME, hour, minute, 0);
                        GTA.Native.Function.Call(GTA.Native.Hash.SET_RANDOM_WEATHER_TYPE);
                    }
                }
            }
        }
        void onKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.O:
                    if (ticker == 0)
                    {
                        GTA.UI.ShowSubtitle("Enabled time circuit editing");
                        ticker++;
                    }
                    else if (ticker == 1)
                    {
                        GTA.UI.ShowSubtitle("Disabled time circuit editing");
                        ticker--;
                    }
                    break;
                case Keys.Up:
                    if (ticker == 1)
                    {
                        hour++;
                        if (hour > 23)
                        {
                            hour = 0;
                        }
                        GTA.UI.ShowSubtitle("Hour: " + hour.ToString());
                    }
                    break;
                case Keys.Down:
                    if (ticker == 1)
                    {
                        hour--;
                        if (hour < 0)
                        {
                            hour = 23;
                        }
                        GTA.UI.ShowSubtitle("Hour: " + hour.ToString());
                    }
                    break;
                case Keys.Left:
                    if (ticker == 1)
                    {
                        minute--;
                        if (minute < 0)
                        {
                            minute = 59;
                        }
                        GTA.UI.ShowSubtitle("Minute: " + minute.ToString());
                    }
                    break;
                case Keys.Right:
                    if (ticker == 1)
                    {
                        minute++;
                        if (minute > 59)
                        {
                            minute = 0;
                        }
                        GTA.UI.ShowSubtitle("Minute: " + minute.ToString());
                    }
                    break;
                case Keys.Z:
                    if (enabled == 1)
                    {
                        enabled--;
                        GTA.UI.ShowSubtitle("Time Travel Disabled");
                    }
                    else if (enabled == 0)
                    {
                        enabled++;
                        GTA.UI.ShowSubtitle("Time Travel Enabled");
                    }
                    break;
            }
        }
    }
}
