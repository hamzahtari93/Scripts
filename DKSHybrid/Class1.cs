using System.Diagnostics;
using System.Threading;
using Rabot.API;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Rabot.Game.Entity;
using NPC = Rabot.API.NPC;

namespace Script
{
    public class DKSHybrid
    {

        //  private const int glacWId = 
        private Keys Turmoil = Keys.O;
        private Keys ANGUISH = Keys.P;
        private Keys TORMENT = Keys.I;
        private Keys SOUL_SPLIT = Keys.U;      
        private Keys DRYGORE_MAINHAND = Keys.NumPad0;
        private Keys OBLITERATION = Keys.NumPad6;
        private Keys WYVERN_CROSSBOW = Keys.NumPad2;
        private Keys OVERLOAD = Keys.NumPad8;       
        private Keys SUPER_RESTORE = Keys.NumPad4;      
        private Keys TELEPORT_WAR = Keys.NumPad5;
        private Keys SUPER_RENEWAL = Keys.NumPad7;
        private Keys SARA_BREW = Keys.V;
        private Keys SHIELD = Keys.C;
        private bool gearSwitch = false;
        private bool isActive = true;      
        private int currPrayer;
        private int initX;
        private int initY;     
        private bool soulsplit;
        private bool hasWithdrawn;
        private Thread potionThread;
        private bool isPotionActive;
        private bool insideArena;
        private bool isFamiliarActive;
        private bool newInstance;
        Thread inactivityThread;
        Thread summoningthread;
        readonly List<int> listName = new List<int>() { 6739, 6730, 6735, 6733, 6737, 6731 };

        public DKSHybrid()
        {

            Rabot.API.Mouse.Scroll.MaxZoomOut();
            //  isPotionActive = false;
            soulsplit = false;
            hasWithdrawn = false;
            newInstance = false;
            gearSwitch = false;
            //   potionThread = new Thread(CheckPotionTime);
            //   potionThread.Start();
            //monitorhealth = new Thread(monitorHealth);
            //monitorhealth.Start();
            //   inactivityThread = new Thread(InactivityController);
            //      inactivityThread.Start();
            //   summoningthread = new Thread(CheckSummoningTime);
            //    summoningthread.Start();

            initX = Players.Self().coordinates.x;
            initY = Players.Self().coordinates.y;
            insideArena = false;
            if (Rabot.API.World.Status() == GameStatus.LoggedOff) { Environment.Exit(0); }
            if (Rabot.API.World.Status() != GameStatus.LoggedIn) { Environment.Exit(0); }
        }

        public void Main()
        {
            var prayer = Rabot.API.HUB.Pray();
            if (Rabot.API.World.Status() != GameStatus.LoggedIn) { Environment.Exit(0); }
            var health = Rabot.API.HUB.Health();
            // Rabot.API.Script.AHconsole("Starting threads");
            //summoningthread.Start();
            //inactivityThread.Start();
            //potionThread.Start();
            while (isActive)
            {
                try
                {


                    if (Inventory.Amount(23399) == 0 && NPC.GetNearest("Death") != null)
                    {
                        retrieveItems();

                    }
                    //  if (!insideArena)
                    bankrun();
                    insideArena = true;

                    if (insideArena)
                    {
                        killboss();
                    }
                }
                catch (Exception ex)
                {

                    //     Rabot.API.Script.AHconsole("Main loop failed");

                }
            }
        }

        public void retrieveItems()
        {

            insideArena = false;
            Rabot.API.Script.AHconsole("Player died, reclaiming items");
            Rabot.API.NPC.GetNearest("Death").Action(MiniMenu.OPTION1);
            Wait();
            Thread.Sleep(new Random().Next(1000, 1800));
            Rabot.Hardware.Keyboard.SendKey(Keys.Space);
            Thread.Sleep(new Random().Next(1000, 1800));
            Rabot.Hardware.Keyboard.SendKey(Keys.NumPad1);
            Thread.Sleep(new Random().Next(1000, 1800));
            if (Interface.IsOpen("DEATH ITEM RECLAIMING"))
            {

                Random random = new Random();
                int num1 = 162;
                int num2 = 518;
                Mouse.Button.Left.Click(num1 + random.Next(-10, 5), num2 + random.Next(-5, 10));
                Thread.Sleep(new Random().Next(2300, 4500));
            }
            if (Interface.IsOpen(""))
            {

                Random random = new Random();
                int num1 = 315;
                int num2 = 351;
                Mouse.Button.Left.Click(num1 + random.Next(-10, 5), num2 + random.Next(-5, 10));
                Thread.Sleep(new Random().Next(2300, 4500));
                Thread.Sleep(new Random().Next(1000, 1800));
                Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                Thread.Sleep(new Random().Next(1000, 1800));
                Rabot.API.Script.AHconsole("Teleporting to War's retreat");
                Rabot.Hardware.Keyboard.SendKey(TELEPORT_WAR);
                Thread.Sleep(new Random().Next(6000, 8000));
            }
            if (Inventory.Amount(556) == 0 && (Inventory.Amount(554) == 0))
            {
                Rabot.API.Script.AHconsole("Retriving from death failed, exiting game");
                { Environment.Exit(0); }
            }
            //Rabot.API.Objects.GetNearest("Bank chest").Action(MiniMenu.OPTION1);
            //Wait();
            //Thread.Sleep(new Random().Next(2000, 3000));
            //Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
            //Thread.Sleep(new Random().Next(2000, 3000));

        }

        public void checkHUB()
        {
            //   try
            {
                var player = Rabot.API.Players.Self();
                double prayer = Rabot.API.HUB.Pray();
                double health = Rabot.API.HUB.Health();
                var restores = Inventory.Get(23399);
                Stopwatch stopwatch = Stopwatch.StartNew();
                Stopwatch potionWatch = Stopwatch.StartNew();


                try
                {


                    if (Inventory.Amount(3024) == 1 && (Inventory.Amount(6685) <= 2) && hasWithdrawn == false)
                    {
                        Rabot.API.Script.AHconsole("Withdrawing from familiar");
                        Rabot.Hardware.Keyboard.SendKey(Keys.NumPad9);
                        Thread.Sleep(new Random().Next(900, 1290));
                        hasWithdrawn = true;


                    }
                }
                catch (Exception ex)
                {
                    //  Rabot.API.Script.AHconsole("Withdrawing super restores failed");

                }
                //try
                //{

                if (!isPotionActive)
                {
                    Rabot.API.Script.AHconsole("Drinking overload/prayer renewal/weapon poison+++");
                    drinkPotion();
                    Thread.Sleep(new Random().Next(1147, 2186));

                }
            
                if (!soulsplit)
                {
                    Rabot.API.Script.AHconsole("Activating soul split");
                    Rabot.Hardware.Keyboard.SendKey(SOUL_SPLIT);
                    Thread.Sleep(new Random().Next(1147, 2186));
                    soulsplit = true;

                }
                try
                {
               if (!isFamiliarActive)
                    {
                        Rabot.API.Script.AHconsole("Summoning Unicorn Stallion and drinking enhanced luck potion");
                        SummonFamiliar();
                        Thread.Sleep(new Random().Next(1147, 2186));
                        isFamiliarActive = true;
                    }
                }
                catch
                {
                    // Rabot.API.Script.AHconsole("Activating familiar failed"); 
                }

                try
                {
                  if (prayer < 700)
                    {
                        //  Rabot.API.Script.AHconsole("Replenishing prayer points");
                        Rabot.Hardware.Keyboard.SendKey(SUPER_RESTORE);
                        Thread.Sleep(new Random().Next(147, 186));
                    }
                }
                catch (Exception ex)
                {
                    //  Rabot.API.Script.AHconsole("Replenishing prayer points failed");

                }

                try
                {
                    if (health < 6000)
                    {
                        //  Rabot.API.Script.AHconsole("Replenishing health");
                        Rabot.Hardware.Keyboard.SendKey(SARA_BREW);
                        Thread.Sleep(new Random().Next(134, 180));

                    }
                }
                catch (Exception ex)
                {
                    //  Rabot.API.Script.AHconsole("Replanishing health failed");

                }

                try
                {
                    while (NPC.GetNearest("Dagannoth Rex") == null && NPC.GetNearest("Dagannoth Supreme") == null && NPC.GetNearest("Dagannoth Prime") == null)
                    {
                        //   Rabot.API.Script.AHconsole("Waiting for NPC to respawn");
                        Thread.Sleep(new Random().Next(1000, 1500));
                        if (stopwatch.Elapsed.Seconds > 30.0)
                        {
                            Rabot.API.Script.AHconsole("Player teleported out or the arena has expired, restarting");
                            Thread.Sleep(new Random().Next(1000, 1500));
                            Rabot.API.Script.AHconsole("Deactivating soulsplit");
                            Rabot.Hardware.Keyboard.SendKey(SOUL_SPLIT);
                            Thread.Sleep(new Random().Next(1000, 1500));
                            insideArena = false;
                            soulsplit = false;
                            isFamiliarActive = false;
                            hasWithdrawn = false;
                            isPotionActive = false;
                            newInstance = false;
                            try
                            {


                                if (Inventory.Amount(23399) <= 1 && NPC.GetNearest("Death") != null)
                                {
                                    retrieveItems();

                                }
                                //  if (!insideArena)
                                bankrun();
                                insideArena = true;

                                if (insideArena)
                                {
                                    killboss();
                                }
                            }
                            catch (Exception)
                            {
                                //      Rabot.API.Script.AHconsole("Restarting loop failed");

                            }
                        }
                    }
                }
                catch
                {
                    //   Rabot.API.Script.AHconsole("Waiting for npc failed");


                }
            }

        }

        public void killPrime1st()
        {
            try
            {

                foreach (var chosenNPC in NPC.Get("Dagannoth Prime")) //select npc
                {
                    //       if ((NPC.GetNearest("Dagannoth Prime") != null))
                    //        {
                    //    Rabot.API.Script.AHconsole("Killing Prime");

                    Rabot.Hardware.Keyboard.SendKey(WYVERN_CROSSBOW);
                    Thread.Sleep(new Random().Next(100, 214));                  
                    Rabot.Hardware.Keyboard.SendKey(ANGUISH);
                    Thread.Sleep(new Random().Next(89, 157));
                    //       MoveCameraRandomly();
                    Thread.Sleep(new Random().Next(100, 200));
                    NPC.Action(chosenNPC.unique_id, MiniMenu.OPTION2);
                }
                while (NPC.GetNearest("Dagannoth Prime") != null)
                    Thread.Sleep(new Random().Next(1060, 1252));

                //     break;
            }

            //     }
            catch (Exception)
            {

                //  Rabot.API.Script.AHconsole("Killing Prime Failed");
            }
        }


        public void killPrime()
        {
            try
            {

                foreach (var chosenNPC in NPC.Get("Dagannoth Prime")) //select npc
                {
                    //       if ((NPC.GetNearest("Dagannoth Prime") != null))
                    //        {
                    //    Rabot.API.Script.AHconsole("Killing Prime");

                    Rabot.Hardware.Keyboard.SendKey(WYVERN_CROSSBOW);
                    Thread.Sleep(new Random().Next(100, 214));
                    Rabot.Hardware.Keyboard.SendKey(SHIELD);
                    Thread.Sleep(new Random().Next(100, 214));
                    Rabot.Hardware.Keyboard.SendKey(ANGUISH);
                    Thread.Sleep(new Random().Next(89, 157));
                    //       MoveCameraRandomly();
                    Thread.Sleep(new Random().Next(100, 200));
                    NPC.Action(chosenNPC.unique_id, MiniMenu.OPTION2);
                }
                while (NPC.GetNearest("Dagannoth Prime") != null)
                    Thread.Sleep(new Random().Next(1060, 1252));

                //     break;
            }

            //     }
            catch (Exception)
            {

                //  Rabot.API.Script.AHconsole("Killing Prime Failed");
            }
        }

        public void killSupreme()
        {

            var player = Rabot.API.Players.Self();
            try
            {


                foreach (var chosenNPC in NPC.Get("Dagannoth Supreme")) //select npc
                {
                    //  if ((NPC.GetNearest("Dagannoth Supreme") != null))
                    //   {
                    //    Rabot.API.Script.AHconsole("Killing Supreme");

                    Rabot.Hardware.Keyboard.SendKey(DRYGORE_MAINHAND);
                    Thread.Sleep(new Random().Next(100, 211));
                    //      Rabot.Hardware.Keyboard.SendKey(DRYGORE_OFFHAND);
                    //      Thread.Sleep(new Random().Next(98, 105));
                    Rabot.Hardware.Keyboard.SendKey(Turmoil);
                    Thread.Sleep(new Random().Next(120, 140));
                    //   MoveCameraRandomly();
                    Thread.Sleep(new Random().Next(100, 200));
                    NPC.Action(chosenNPC.unique_id, MiniMenu.OPTION2);
                    Thread.Sleep(new Random().Next(600, 1850));
                }
                while (NPC.GetNearest("Dagannoth Supreme") != null)
                    Thread.Sleep(new Random().Next(1060, 1252));
                //  break;
            }

            //   }


            catch (Exception)
            {

                //  Rabot.API.Script.AHconsole("Killing Supreme failed");

            }
        }

        public void killRex()
        {
            try
            {

                foreach (var chosenNPC in NPC.Get("Dagannoth Rex")) //select npc
                {
                    //      if ((NPC.GetNearest("Dagannoth Rex") != null))
                    //     {
                    //   Rabot.API.Script.AHconsole("Killing Rex");

                    Rabot.Hardware.Keyboard.SendKey(OBLITERATION);
                    Thread.Sleep(new Random().Next(154, 186));
                    Rabot.Hardware.Keyboard.SendKey(TORMENT);
                    Thread.Sleep(new Random().Next(125, 130));
                    Thread.Sleep(new Random().Next(100, 200));
                    NPC.Action(chosenNPC.unique_id, MiniMenu.OPTION2);
                }
                while (NPC.GetNearest("Dagannoth Rex") != null)
                    Thread.Sleep(new Random().Next(1060, 1252));
                // break;
            }


            //   }
            //   while (NPC.Get("Dagannoth Rex").First().current_health > 0) {; }


            //       Thread.Sleep(new Random().Next(200, 300));
            //    return;
            //if (player.state == PlayerState.IDLE && stopwatch.Elapsed.Seconds > 60.0)
            //{
            //    Rabot.API.Script.AHconsole("killRex() loop stuck , going back to main()");


            catch (Exception)
            {

                //  Rabot.API.Script.AHconsole("Killing Rex failed");

            }
        }
        public void killboss()
        {
            //  try
            {
                var prayer = Rabot.API.HUB.Pray();
                var health = Rabot.API.HUB.Health();
                var restores = Rabot.API.Inventory.Get(23399);
                var localplayer = Players.Self();
                int i = 0;
                while (insideArena)
                {
                    if (i == 3)
                    {
                        drinkPotion();
                        i = 0;
                    }
                    MoveCameraRandomly();
                    checkHUB();                                   
                    killPrime();
                    lootSeers();
                    lootDragonH();
                    lootArchers();
                    lootBerserker();
                    lootWarrior();
                    checkHUB();
                    killSupreme();
                    lootSeers();
                    lootDragonH();
                    lootArchers();
                    lootBerserker();
                    lootWarrior();
                    //MoveCameraRandomly();
                    checkHUB();
                    killRex();
                    lootSeers();
                    lootDragonH();
                    lootArchers();
                    lootBerserker();
                    lootWarrior();
                    i++;

                }
            }
            //   catch (Exception ex)
            {

                //Rabot.API.Script.AHconsole("Killboss function failed, exiting");
                //Rabot.Hardware.Keyboard.SendKey(TELEPORT_WAR);
                //Thread.Sleep(new Random().Next(600, 900));
                //{ Environment.Exit(0); }

            }
        }






        public void bankrun()
        {
            try
            {
                
                var localplayer = Players.Self();
                initX = Players.Self().coordinates.x;
                initY = Players.Self().coordinates.y;
                if (NPC.GetNearest("War") != null)
                {
                    restart();
                }
                else
                {
                    Rabot.API.Script.AHconsole("Starting bankrun");
                    Rabot.Hardware.Keyboard.SendKey(TELEPORT_WAR);
                    Thread.Sleep(new Random().Next(6000, 9000));
                    //  while (!(NPC.Get("War") != null))
                    //   insideArena = false;
                    Rabot.API.Objects.GetNearest("Altar of War").Action(MiniMenu.OPTION1);
                    Wait();
                    Rabot.API.Objects.GetNearest("Bank chest").Action(MiniMenu.OPTION1);
                    Wait();
                    Thread.Sleep(new Random().Next(2000, 3000));
                    Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
                    Thread.Sleep(new Random().Next(2000, 3000));

                    Rabot.API.Objects.GetNearest(114762).Action(MiniMenu.OPTION1);
                    Wait();
                    Thread.Sleep(new Random().Next(4000, 6000));
                    Rabot.API.Script.AHconsole(" Starting instance");
                    Rabot.API.Objects.GetNearest("Ladder").Action(MiniMenu.OPTION1);
                    Wait();
                    Thread.Sleep(new Random().Next(2548, 3687));
                    Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
                    Thread.Sleep(new Random().Next(1987, 2658));
                    if (Interface.IsOpen("INSTANCE SYSTEM"))
                    {

                        Random random = new Random();
                        int num1 = 291;
                        int num2 = 402;
                        Mouse.Button.Left.Click(num1 + random.Next(-5, 5), num2 + random.Next(-5, 5));
                        Thread.Sleep(new Random().Next(2300, 4500));
                        Thread.Sleep(new Random().Next(1500, 2000));
                        Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                        Thread.Sleep(new Random().Next(1500, 2000));
                        Rabot.Hardware.Keyboard.SendKey(Keys.NumPad1);
                        Thread.Sleep(new Random().Next(1500, 2000));
                    //    if (initX != 1912 && initY != 4367)
                    //    {
                    //        newInstance = true;
                    //    }
                    //}
                    //if (newInstance == false)
                    //{
                    //    Rabot.API.Script.AHconsole("Instance already active");
                    //    Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                    //    Thread.Sleep(new Random().Next(1500, 2000));
                    //    Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
                    //    Thread.Sleep(new Random().Next(1500, 2000));
                    //    Rabot.API.Script.AHconsole("Rejoining old instance");
                    //    Random randoms = new Random();
                    //    int num3 = 588;
                    //    int num4 = 404;
                    //    Mouse.Button.Left.Click(num3 + randoms.Next(-10, 10), num4 + randoms.Next(-8, 9));
                    //    Thread.Sleep(new Random().Next(4300, 6500));
                    //    newInstance = true;
                        //else if (Objects.GetNearest("Rocks") != null)
                        //{
                        //    newInstance = true;
                        //    Rabot.API.Script.AHconsole("Created new instance");
                        //}
                        //       return;

                        //else if (Interface.IsOpen("") && newInstance == false)

                        //{
                        //    Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                        //    Thread.Sleep(new Random().Next(1500, 2000));
                        //    Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
                        //    Thread.Sleep(new Random().Next(1500, 2000));
                        //    Rabot.API.Script.AHconsole("Rejoining old instance");
                        //    Random randoms = new Random();
                        //    int num3 = 588;
                        //    int num4 = 404;
                        //    Mouse.Button.Left.Click(num3 + randoms.Next(-5, 5), num4 + randoms.Next(-5, 5));
                        //    Thread.Sleep(new Random().Next(4300, 6500));
                        //    return;

                        //else
                        //{

                        //    //  Thread.Sleep(new Random().Next(5000, 8000));
                        //    return;
                    }
                }
            }



            catch
            {
                //  Rabot.API.Script.AHconsole("Bankrun failed"); 
            }
        }

        public void restart()
        {


            Rabot.API.Objects.GetNearest("Altar of War").Action(MiniMenu.OPTION1);
            Wait();
            Rabot.API.Objects.GetNearest("Bank chest").Action(MiniMenu.OPTION1);
            Wait();
            Thread.Sleep(new Random().Next(2000, 3000));
            Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
            Thread.Sleep(new Random().Next(2000, 3000));

            Rabot.API.Objects.GetNearest(114762).Action(MiniMenu.OPTION1);
            Wait();
            Thread.Sleep(new Random().Next(4000, 6000));
            Rabot.API.Script.AHconsole(" Starting instance");
            Rabot.API.Objects.GetNearest("Ladder").Action(MiniMenu.OPTION1);
            Wait();
            Thread.Sleep(new Random().Next(2548, 3687));
            Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
            Thread.Sleep(new Random().Next(1987, 2658));
            if (Interface.IsOpen("INSTANCE SYSTEM"))
            {

                Random random = new Random();
                int num1 = 291;
                int num2 = 402;
                Mouse.Button.Left.Click(num1 + random.Next(-5, 5), num2 + random.Next(-5, 5));
                Thread.Sleep(new Random().Next(4300, 6500));
                Thread.Sleep(new Random().Next(1500, 2000));
                Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                Thread.Sleep(new Random().Next(1500, 2000));
                Rabot.Hardware.Keyboard.SendKey(Keys.NumPad1);
                Thread.Sleep(new Random().Next(1500, 2000));
            //    if (initX != 1912 && initY != 4367)
            //    {
            //        newInstance = true;
            //    }
            //}
            //  if (newInstance == false)
            //    {
              
            //    Rabot.API.Script.AHconsole("Rejoined instance");
            //    Rabot.Hardware.Keyboard.SendKey(Keys.Space);
            //    Thread.Sleep(new Random().Next(1500, 2000));
            //    Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
            //    Thread.Sleep(new Random().Next(1500, 2000));
            //    Rabot.API.Script.AHconsole("Clicking rejoin");
            //    Random randoms = new Random();
            //    int num3 = 588;
            //    int num4 = 404;
            //    Mouse.Button.Left.Click(num3 + randoms.Next(-5, 5), num4 + randoms.Next(-5, 5));
            //    Thread.Sleep(new Random().Next(2300, 4500));
                
               // return;

            
        
    
                  //else  if (Objects.GetNearest("Rocks") != null)
                  //  {
                  //      newInstance = true;
                  //      Rabot.API.Script.AHconsole("Created new instance");
                  //  }
                    //  return;                

                    //else if (Interface.IsOpen("") && newInstance == false)

                    //   {
                    //       Rabot.Hardware.Keyboard.SendKey(Keys.Space);
                    //       Thread.Sleep(new Random().Next(1500, 2000));
                    //       Rabot.Hardware.Keyboard.SendKey(Keys.NumPad2);
                    //       Thread.Sleep(new Random().Next(1500, 2000));
                    //       Rabot.API.Script.AHconsole("Clicking rejoin");
                    //       Random randoms = new Random();
                    //       int num3 = 588;
                    //       int num4 = 404;
                    //       Mouse.Button.Left.Click(num3 + randoms.Next(-5, 5), num4 + randoms.Next(-5, 5));
                    //       Thread.Sleep(new Random().Next(2300, 4500));
                    //       return;
                    //   }
                }
            }
        









        public void Checkfloor()
        {
            try
            {
                // Thread.Sleep(new Random().Next(1750, 2460));

                foreach (var floorLoot in GroundItems.Get())
                {
                    List<int> noteList = listName; //my list of items to note
                    Stack chosenStack = floorLoot.stack; //converts groundloot into a stack
                    GameCoordinates lootLocation = floorLoot.coordinates;  //assigns coords to each groundloot                    
                    WaitMovement();

                    for (int i = 0; i < chosenStack.amount; i++)
                    {
                        var Stacki = chosenStack.item[i];
                        if (noteList.Contains(Stacki.id))
                        {
                            Rabot.API.Script.AHconsole(floorLoot.stack.ToString());
                            //   Rabot.API.GroundItems.Action(floorLoot.stack.item.First().id, floorLoot.coordinates, MiniMenu.OPTION1);
                            GroundItems.Action(Stacki.id, lootLocation, MiniMenu.OPTION1); //picks up item
                            WaitMovement();
                        }
                    }
                }
            }
            catch
            { }
        }
        public void DeactivatePrayer()
        {
            switch (currPrayer)
            {
                case 1:
                    Rabot.Hardware.Keyboard.SendKey(ANGUISH);
                    break;
                case 2:
                    Rabot.Hardware.Keyboard.SendKey(Turmoil);
                    break;
                case 3:
                    Rabot.Hardware.Keyboard.SendKey(TORMENT);
                    break;
                case 4:
                    Rabot.Hardware.Keyboard.SendKey(SOUL_SPLIT);
                    break;
            }
            currPrayer = 0;
        }

        public void lootDragonH()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6739))
                    while (chosenItem != null)
                    {
                        //    var dragonH = Rabot.API.GroundItems.Get(6739).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);
                        Rabot.API.Script.AHconsole("Looting dragon hatchet");
                        WaitMovement();
                        return;
                    }
            }
            catch
            {
                //  Rabot.API.Script.AHconsole("Looting Dragon hatchet failed, exiting");
                //  Rabot.Hardware.Keyboard.SendKey(TELEPORT_WAR);
                //  Thread.Sleep(new Random().Next(600, 900));
                //  { Environment.Exit(0); }
            }
        }

        public void lootBonesNoted()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6730))
                    if (chosenItem != null)
                    {

                        //    var chosenItem = Rabot.API.GroundItems.Get(6730).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);

                        WaitMovement();
                        Rabot.API.Script.AHconsole("Looting noted bones");
                        return;
                    }
            }
            catch (Exception ex)
            {
                //   Rabot.API.Script.AHconsole("Looting noted bones failed");

            }
        }

        public void lootBones()
        {

            foreach (var chosenItem in GroundItems.Get(6729))
                if (chosenItem != null)
                {
                    // var chosenItem = Rabot.API.GroundItems.Get(6729).First();
                    Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);
                    WaitMovement();
                    return;
                }
        }

        public void lootWarrior()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6735))
                    while (chosenItem != null)
                    {

                        //    var chosenItem = Rabot.API.GroundItems.Get(6735).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);

                        WaitMovement();
                        Rabot.API.Script.AHconsole("Looting Warriors ring");
                        return;
                    }
            }
            catch (Exception ex)
            {
                //   Rabot.API.Script.AHconsole("Looting warrior failed, trying again");

            }
        }
        public void lootArchers()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6733))
                    while (chosenItem != null)
                    {

                        //  var chosenItem = Rabot.API.GroundItems.Get(6733).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);

                        WaitMovement();
                        Rabot.API.Script.AHconsole("Looting Archers ring");
                        return;
                    }
            }
            catch (Exception ex)
            {
                //    Rabot.API.Script.AHconsole("looting archers failed");

            }
        }

        public void lootBerserker()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6737))
                    while (chosenItem != null)
                    {


                        //   var chosenItem = Rabot.API.GroundItems.Get(6737).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);

                        WaitMovement();
                        Rabot.API.Script.AHconsole("Looting Berserker ring");
                        return;
                    }
            }
            catch (Exception ex)
            {
                //  Rabot.API.Script.AHconsole("looting berserker failed");

            }
        }
        public void lootSeers()
        {
            try
            {
                foreach (var chosenItem in GroundItems.Get(6731))
                    while (chosenItem != null)
                    {


                        //   var chosenItem = Rabot.API.GroundItems.Get(6731).First();
                        Rabot.API.GroundItems.Action(chosenItem.stack.item.First().id, chosenItem.coordinates, MiniMenu.OPTION1);

                        WaitMovement();
                        Rabot.API.Script.AHconsole("Looting Seers ring");
                        return;
                    }
            }
            catch (Exception ex)
            {
                //  Rabot.API.Script.AHconsole("looting seers failed");

            }
        }
        public void wait()
        {
            var player = Rabot.API.Players.Self();
            while (player.state == PlayerState.RUNNING)
            {
                Thread.Sleep(500);
            }


        }

        public void drinkPotion()
        {

            Rabot.Hardware.Keyboard.SendKey(OVERLOAD);
            Thread.Sleep(new Random().Next(1106, 1600));
            //  Rabot.Hardware.Keyboard.SendKey(WEAPON_POISON);
            // Thread.Sleep(new Random().Next(999, 1500));
            Rabot.Hardware.Keyboard.SendKey(SUPER_RENEWAL);
            Thread.Sleep(new Random().Next(999, 1500));
            isPotionActive = true;

        }

        public void CheckPotionTime()
        {

            var prayer = Rabot.API.HUB.Pray();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {

                if (stopwatch.Elapsed.TotalMinutes > 5.30)
                {
                    isPotionActive = false;
                    while (!isPotionActive)
                        Thread.Sleep(1000);
                    stopwatch.Restart();
                }

                else if (stopwatch.Elapsed.TotalMinutes > 60.0)
                {
                    isPotionActive = false;
                    while (!isPotionActive)
                        Thread.Sleep(1000);
                    stopwatch.Restart();
                }
                Thread.Sleep(2500);
            }
        }

        public void SummonFamiliar()
        {

            Rabot.Hardware.Keyboard.SendKey(Keys.Q);
            Thread.Sleep(new Random().Next(800, 1000));
            Rabot.Hardware.Keyboard.SendKey(Keys.K);
            Thread.Sleep(new Random().Next(800, 1000));

            //  Keyboard.SendKey(Keys.F1);
        }


        public void CheckPotionsTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            // while (true)
            {
                if (stopwatch.Elapsed.TotalMinutes > 5.50)
                {
                    isPotionActive = false;
                    drinkPotion();
                    //   while (!isPotionActive)

                    //      Thread.Sleep(1000);
                    stopwatch.Restart();
                }
                //    Thread.Sleep(2500);
            }
        }
        public void CheckSummoningTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (stopwatch.Elapsed.TotalMinutes > 60.0)
                {
                    isFamiliarActive = false;
                    while (!isFamiliarActive)
                        Thread.Sleep(1000);
                    stopwatch.Restart();
                }
                Thread.Sleep(2500);
            }
        }

        private void InactivityController()
        {

            int initExp = Rabot.API.Skill.Get(Ability.HITPOINT).experience;

            while (true)
            {
                Thread.Sleep(150000);
                if (initExp == Rabot.API.Skill.Get(Ability.HITPOINT).experience)

                {
                    Rabot.API.Script.AHconsole("Player teleported out or the arena has expired");
                    //     Rabot.Hardware.Keyboard.SendKey(TELEPORT_WAR);
                    //     Thread.Sleep(new Random().Next(9060, 10030));
                    Rabot.Hardware.Keyboard.SendKey(SOUL_SPLIT);
                    insideArena = false;
                    soulsplit = false;
                    isFamiliarActive = false;




                }
            }
        }

        public void Wait()
        {
            var player = Rabot.API.Players.Self();

            WaitMovement();

            while (player.Update().animation != -1)
                Thread.Sleep(2000);
        }
        private void WaitMovement()
        {
            int securityCounter = 0;
            var player = Rabot.API.Players.Self();

            Thread.Sleep(250);

            do
            {
                Thread.Sleep(500);

                if (player.Update().idle) ++securityCounter;

            } while (securityCounter <= 2);
        }

        private void MoveCameraRandomly()
        {
            try
            {
                if (Rabot.API.World.Status() != GameStatus.LoggedIn) { Environment.Exit(0); }
                int minValue = 150;
                int maxValue = 650;
                int num = new Random().Next(15, 25);
                int num2 = 0;
                int num3 = num2;
                bool flag = num <= num3;
                if (flag)
                {
                    int num4 = num2 + 1;
                }
                else
                {
                    int num5 = new Random().Next(0, 3);
                    int num6 = new Random().Next(0, 2);
                    int num7 = num5;
                    int num8 = num7;
                    if (num8 != 0)
                    {
                        if (num8 == 1)
                        {
                            Rabot.Hardware.Keyboard.HoldKey(Keys.Right, new Random().Next(minValue, maxValue));
                            bool flag2 = num6 == 0;
                            if (flag2)
                            {
                                Rabot.Hardware.Keyboard.HoldKey(Keys.Left, new Random().Next(minValue, maxValue));
                            }
                            else
                            {
                                Rabot.Hardware.Keyboard.HoldKey(Keys.Left, new Random().Next(minValue, maxValue));
                            }
                        }
                    }
                    else
                    {
                        Rabot.Hardware.Keyboard.HoldKey(Keys.Right, new Random().Next(minValue, maxValue));
                        bool flag3 = num6 == 0;
                        if (flag3)
                        {
                            Rabot.Hardware.Keyboard.HoldKey(Keys.Right, new Random().Next(minValue, maxValue));
                        }
                        else
                        {
                            Rabot.Hardware.Keyboard.HoldKey(Keys.Left, new Random().Next(minValue, maxValue));
                        }
                    }
                }
            }
            catch (Exception ex) { Rabot.API.Script.AHconsole("Camera movements failed"); }
        }
    }
}












