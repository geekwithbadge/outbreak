﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace Outbreak.Core
{
    public class Panel : BaseScript
    {
        public Panel()
        {
            Menu MainMenu = new Menu("Admin Menu", "Manage the server")
            {
                TitleFont = 2,
                HeaderColor = new int[] { 199, 0, 57, 255 }
            };
 
            MainMenu.Register(MainMenu);
            MainMenu.AddItem("Get Coords", "Prints on the screen your own coords.");
            MainMenu.AddItem("TP Marker", "Teleport to the marker.");

            List<string> What = new List<string>
            {
                "WEAPON_PISTOL",
                "WEAPON_ASSAULTRIFLE",
                "WEAPON_BAT"
            };
            MainMenu.AddItemList("Get Weapon", "Get a weapon.", What);


            MainMenu.OnItemSelect += (name, index) =>
            {
                if (index == 1)
                {
                    GetCoords();
                }
                else if (index == 2)
                {
                    TPMarker();
                }
            };

            MainMenu.OnItemListSelect += (name, index, namelist, indexlist) =>
            {
                for (int i = 0; i <= What.Count; i++)
                {
                    if (index == 3 && indexlist == i)
                    {
                        GiveWeapon(namelist);
                    }
                }
                

            };

            Tick += async () =>
            {
                MainMenu.Initiation();

                if (Game.IsControlJustPressed(0, Control.InteractionMenu) && !MainMenu.IsAnyMenuOpen())
                {
                    MainMenu.InteractMenu();
                }

                await Task.FromResult(0);
            };

            Testcommand();

        }

        private async void TPMarker()
        {
            var Mark = GetFirstBlipInfoId(8);

            if (DoesBlipExist(Mark))
            {
                var MarkCoords = GetBlipInfoIdCoord(Mark);
                Convert.ToSingle(MarkCoords.X);
                Convert.ToSingle(MarkCoords.Y);
                Convert.ToSingle(MarkCoords.Z);
                float Default = 0.0f;

                for (int i = 1; i < 999; i++)
                {
                    SetPedCoordsKeepVehicle(PlayerPedId(), MarkCoords.X, MarkCoords.Y, i);

                    bool GetGroundZ = GetGroundZFor_3dCoord(MarkCoords.X, MarkCoords.Y, i, ref Default, false);

                    if (GetGroundZ)
                    {
                        SetPedCoordsKeepVehicle(PlayerPedId(), MarkCoords.X, MarkCoords.Y, i + 0.3f);
                        break;
                    }
                    await Delay(10);
                }
                Screen.ShowNotification("~b~You have teleported~b~");
            }
            else
            {
                Screen.ShowNotification("~b~There is no marker to teleport~b~");
            }

        }

        private void GetCoords()
        {
            Vector3 playercoords = GetEntityCoords(PlayerPedId(), true);
            Debug.WriteLine($"^1[Outbreak.Core.Admin]^7: {playercoords}");
        }

        private void GiveWeapon(string Weapon)
        {
            GiveWeaponToPed(PlayerPedId(), (uint)GetHashKey(Weapon), 250, false, true);
        }

        private void Testcommand()
        {
            RegisterCommand("test", new Action<int, List<object>, string>((source, args, raw) =>
            {
                

            }), false);
            
        }

        
    }
}
