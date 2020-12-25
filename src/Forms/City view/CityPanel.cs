﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using civ2.Bitmaps;
using civ2.Units;
using civ2.Improvements;

namespace civ2.Forms
{
    public partial class CityPanel : Civ2panel
    {
        private Game Game => Game.Instance;
        private Map Map => Map.Instance;

        private Main _main;
        private readonly City _thisCity;
        private readonly DoubleBufferedPanel _resourceMap;
        private Bitmap CityDrawing;
        private DoubleBufferedPanel WallpaperPanel, CityResources, UnitsFromCity, UnitsInCity, FoodStorage, ProductionPanel;
        private Form CallingForm;
        private VScrollBar ImprovementsBar;
        private int[,] offsets;
        private int ProductionItem;

        public CityPanel(Main parent, City city, int _width, int _height) : base(_width, _height, "", 27, 11)   // TODO: correct padding for max/min zoom
        {
            _main = parent;
            _thisCity = city;

            // Add DrawPanel from base control
            Controls.Add(DrawPanel);
            DrawPanel.BackgroundImage = Images.CityWallpaper;
            DrawPanel.Paint += DrawPanel_Paint;

            //this.Load += new EventHandler(CityForm_Load);
            this.Paint += CityPanel_Paint;

            //Panel for wallpaper
            //WallpaperPanel = new DoubleBufferedPanel
            //{
            //    Location = new Point(12, 37),    //normal zoom = (8,25)
            //    Size = new Size(960, 630),      //normal zoom = (640,420)
            //    //BackgroundImage = ModifyImage.ResizeImage(Images.CityWallpaper, 960, 630),    // TODO: correct this
            //};
            //Controls.Add(WallpaperPanel);
            //WallpaperPanel.Paint += new PaintEventHandler(WallpaperPanel_Paint);
            //WallpaperPanel.Paint += new PaintEventHandler(ImprovementsList_Paint);

            // Faces panel
            var _faces = new DoubleBufferedPanel
            {
                Location = new Point(3, 2),
                Size = new Size(433, 57),
                BackColor = Color.Transparent
            };
            DrawPanel.Controls.Add(_faces);
            _faces.Paint += Faces_Paint;

            // Resource map panel
            _resourceMap = new DoubleBufferedPanel
            {
                Location = new Point(5, 84),
                Size = new Size(4 * 48, 4 * 24),
                BackColor = Color.Transparent
            };
            DrawPanel.Controls.Add(_resourceMap);
            _resourceMap.Paint += ResourceMap_Paint;

            ////City resources panel
            //CityResources = new DoubleBufferedPanel
            //{
            //    Location = new Point(300, 70),
            //    Size = new Size(350, 245),    //stretched by 12.5 %
            //    BackColor = Color.Transparent
            //};
            //WallpaperPanel.Controls.Add(CityResources);
            //CityResources.Paint += new PaintEventHandler(CityResources_Paint);

            ////Units from city panel
            //UnitsFromCity = new DoubleBufferedPanel
            //{
            //    Location = new Point(10, 321),
            //    Size = new Size(270, 104),
            //    BackColor = Color.Transparent
            //};
            //WallpaperPanel.Controls.Add(UnitsFromCity);
            //UnitsFromCity.Paint += new PaintEventHandler(UnitsFromCity_Paint);

            ////Units in city panel
            //UnitsInCity = new DoubleBufferedPanel
            //{
            //    Location = new Point(288, 322),
            //    Size = new Size(360, 245),
            //    BackColor = Color.Transparent
            //};
            //WallpaperPanel.Controls.Add(UnitsInCity);
            //UnitsInCity.Paint += new PaintEventHandler(UnitsInCity_Paint);

            ////Food storage panel
            //FoodStorage = new DoubleBufferedPanel
            //{
            //    Location = new Point(653, 0),
            //    Size = new Size(291, 244),
            //    BackColor = Color.Transparent
            //};
            //WallpaperPanel.Controls.Add(FoodStorage);
            //FoodStorage.Paint += new PaintEventHandler(FoodStorage_Paint);

            ////Production panel
            //ProductionPanel = new DoubleBufferedPanel
            //{
            //    Location = new Point(657, 249),
            //    Size = new Size(293, 287),
            //    BackColor = Color.Transparent
            //};
            //WallpaperPanel.Controls.Add(ProductionPanel);
            //ProductionPanel.Paint += new PaintEventHandler(ProductionPanel_Paint);

            ////Buy button
            //Civ2button BuyButton = new Civ2button
            //{
            //    Location = new Point(8, 24),
            //    Size = new Size(102, 36),
            //    Font = new Font("Arial", 13),
            //    Text = "Buy"
            //};
            //ProductionPanel.Controls.Add(BuyButton);
            //BuyButton.Click += new EventHandler(BuyButton_Click);

            ////Change button
            //Civ2button ChangeButton = new Civ2button
            //{
            //    Location = new Point(180, 24),
            //    Size = new Size(102, 36),
            //    Font = new Font("Arial", 13),
            //    Text = "Change"
            //};
            //ProductionPanel.Controls.Add(ChangeButton);
            //ChangeButton.Click += new EventHandler(ChangeButton_Click);

            ////Info button
            //Civ2button InfoButton = new Civ2button
            //{
            //    Location = new Point(692, 549), //original (461, 366)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "Info"
            //};
            //WallpaperPanel.Controls.Add(InfoButton);
            //InfoButton.Click += new EventHandler(InfoButton_Click);

            ////Map button
            //Civ2button MapButton = new Civ2button
            //{
            //    Location = new Point(779, 549), //original (519, 366)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "Map"
            //};
            //WallpaperPanel.Controls.Add(MapButton);
            //MapButton.Click += new EventHandler(MapButton_Click);

            ////Rename button
            //Civ2button RenameButton = new Civ2button
            //{
            //    Location = new Point(866, 549), //original (577, 366)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "Rename"
            //};
            //WallpaperPanel.Controls.Add(RenameButton);
            //RenameButton.Click += new EventHandler(RenameButton_Click);

            ////Happy button
            //Civ2button HappyButton = new Civ2button
            //{
            //    Location = new Point(692, 587), //original (461, 391)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "Happy"
            //};
            //WallpaperPanel.Controls.Add(HappyButton);
            //HappyButton.Click += new EventHandler(HappyButton_Click);

            ////View button
            //Civ2button ViewButton = new Civ2button
            //{
            //    Location = new Point(779, 587), //original (519, 391)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "View"
            //};
            //WallpaperPanel.Controls.Add(ViewButton);
            //ViewButton.Click += new EventHandler(ViewButton_Click);

            ////Exit button
            //Civ2button ExitButton = new Civ2button
            //{
            //    Location = new Point(866, 587), //original (577, 391)
            //    Size = new Size(86, 36),  //original (57, 24)
            //    Font = new Font("Arial", 13),
            //    Text = "Exit"
            //};
            //WallpaperPanel.Controls.Add(ExitButton);
            //ExitButton.Click += new EventHandler(ExitButton_Click);

            ////Next city (UP) button
            //NoSelectButton NextCityButton = new NoSelectButton
            //{
            //    Location = new Point(660, 550), //original (440, 367)
            //    Size = new Size(32, 36),  //original (21, 24)
            //    BackColor = Color.FromArgb(107, 107, 107)
            //};
            //NextCityButton.FlatStyle = FlatStyle.Flat;
            //WallpaperPanel.Controls.Add(NextCityButton);
            //NextCityButton.Click += new EventHandler(NextCityButton_Click);
            //NextCityButton.Paint += new PaintEventHandler(NextCityButton_Paint);

            ////Previous city (DOWN) button
            //NoSelectButton PrevCityButton = new NoSelectButton
            //{
            //    Location = new Point(660, 588), //original (440, 392)
            //    Size = new Size(32, 36),  //original (21, 24)
            //    BackColor = Color.FromArgb(107, 107, 107)
            //};
            //PrevCityButton.FlatStyle = FlatStyle.Flat;
            //WallpaperPanel.Controls.Add(PrevCityButton);
            //PrevCityButton.Click += new EventHandler(PrevCityButton_Click);
            //PrevCityButton.Paint += new PaintEventHandler(PrevCityButton_Paint);

            ////Improvements vertical bar
            //ImprovementsBar = new VScrollBar()
            //{
            //    Location = new Point(270, 433),
            //    Size = new Size(15, 190),
            //    Maximum = 66 - 9 + 9    //max improvements=66, 9 can be shown, because of slider size it's 9 elements smaller
            //};
            //WallpaperPanel.Controls.Add(ImprovementsBar);
            //ImprovementsBar.ValueChanged += new EventHandler(ImprovementsBarValueChanged);

            ////Initialize city drawing
            ////CityDrawing = Draw.DrawCityFormMap(ThisCity);

            ////Define offset map array
            //offsets = new int[20, 2] { { -2, 0 }, { -1, -1 }, { 0, -2 }, { 1, -1 }, { 2, 0 }, { 1, 1 }, { 0, 2 }, { -1, 1 }, { -3, -1 }, { -2, -2 }, { -1, -3 }, { 1, -3 }, { 2, -2 }, { 3, -1 }, { 3, 1 }, { 2, 2 }, { 1, 3 }, { -1, 3 }, { -2, 2 }, { -3, 1 } };

            //ProductionItem = 0; //item appearing in production menu on loadgame
        }

        //private void CityForm_Load(object sender, EventArgs e)
        //{
        //    Location = new Point(CallingForm.Width / 2 - this.Width / 2, CallingForm.Height / 2 - this.Height / 2 + 60);
        //}

        private void CityPanel_Paint(object sender, PaintEventArgs e)
        {
            var sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            string bcad = (Game.GameYear < 0) ? "B.C." : "A.D.";
            string text = String.Format($"City of {_thisCity.Name}, {Math.Abs(Game.GameYear)} {bcad}, Population {_thisCity.Population:n0} (Treasury: {_thisCity.Owner.Money} Gold)");

            e.Graphics.DrawString(text, new Font("Times New Roman", 14), new SolidBrush(Color.Black), new Point((Width / 2) + 1, 15), sf);
            e.Graphics.DrawString(text, new Font("Times New Roman", 14), new SolidBrush(Color.FromArgb(135, 135, 135)), new Point(Width / 2, 15), sf);
            sf.Dispose();
        }

        private void DrawPanel_Paint(object sender, PaintEventArgs e)
        {
        }

        // Draw faces
        private void Faces_Paint(object sender, PaintEventArgs e)
        {
            // Image of faces
            e.Graphics.DrawImage(Draw.Citizens(_thisCity, 0), 2, 7);
            // Text
            var sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            e.Graphics.DrawString("Citizens", new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(67, 67, 67)), new Point(98 + 1, 51 + 1), sf);
            e.Graphics.DrawString("Citizens", new Font("Arial", 9, FontStyle.Bold), new SolidBrush(Color.FromArgb(223, 187, 63)), new Point(98, 51), sf);
            sf.Dispose();
        }

        // Draw resource map
        private void ResourceMap_Paint(object sender, PaintEventArgs e)
        {
            // Map around city
            e.Graphics.DrawImage(Draw.CityResourcesMap(_thisCity, -2), 0, 0);
            //e.Graphics.DrawImage(ModifyImage.ResizeImage(CityDrawing, -2), 0, 0);
            //Food/shield/trade icons around the city (21 of them altogether)
            for (int i = 0; i <= _thisCity.Size; i++)
            {
                //e.Graphics.DrawImage(Draw.DrawCityFormMapIcons(ThisCity, ThisCity.PriorityOffsets[i, 0], ThisCity.PriorityOffsets[i, 1]), 36 * (ThisCity.PriorityOffsets[i, 0] + 3) + 13, 18 * (ThisCity.PriorityOffsets[i, 1] + 3) + 11);
            }
        }

        //Once slider value changes --> redraw improvements list
        //private void ImprovementsBarValueChanged(object sender, EventArgs e)
        //{
        //    WallpaperPanel.Invalidate();
        //}

        //private void WallpaperPanel_Paint(object sender, PaintEventArgs e)
        //{
        //    //Borders of panel
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), 0, WallpaperPanel.Height - 2, WallpaperPanel.Width, WallpaperPanel.Height - 2);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), 0, WallpaperPanel.Height - 1, WallpaperPanel.Width, WallpaperPanel.Height - 1);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), WallpaperPanel.Width - 2, 0, WallpaperPanel.Width - 2, WallpaperPanel.Height);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(223, 223, 223)), WallpaperPanel.Width - 1, 0, WallpaperPanel.Width - 1, WallpaperPanel.Height);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 0, 0, WallpaperPanel.Width - 2, 0);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 0, 1, WallpaperPanel.Width - 3, 1);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 0, 0, 0, WallpaperPanel.Height - 2);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(67, 67, 67)), 1, 0, 1, WallpaperPanel.Height - 3);

        //    //Texts
        //    e.Graphics.DrawString("Resource Map", new Font("Arial", 13), new SolidBrush(Color.FromArgb(243, 183, 7)), new Point(90, 280));
        //    e.Graphics.DrawString("City Resources", new Font("Arial", 13), new SolidBrush(Color.FromArgb(243, 183, 7)), new Point(400, 70));
        //    e.Graphics.DrawString("City Improvements", new Font("Arial", 13), new SolidBrush(Color.FromArgb(223, 187, 7)), new Point(56, 433));
        //}

        ////Draw city resources
        //private void CityResources_Paint(object sender, PaintEventArgs e)
        //{
        //    StringFormat sf1 = new StringFormat();
        //    StringFormat sf2 = new StringFormat();
        //    sf1.Alignment = StringAlignment.Far;
        //    sf2.Alignment = StringAlignment.Center;

        //    //Draw food+surplus/hunger strings
        //    e.Graphics.DrawString("Food: " + _thisCity.Food.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(6, 21));
        //    e.Graphics.DrawString("Food: " + _thisCity.Food.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(87, 171, 39)), new Point(5, 20));
        //    e.Graphics.DrawString("Surplus: " + _thisCity.SurplusHunger.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(346, 21), sf1);
        //    e.Graphics.DrawString("Surplus: " + _thisCity.SurplusHunger.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(63, 139, 31)), new Point(345, 20), sf1);

        //    //Draw trade+corruption strings
        //    e.Graphics.DrawString("Trade: " + _thisCity.Trade.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(6, 83));
        //    e.Graphics.DrawString("Trade: " + _thisCity.Trade.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(239, 159, 7)), new Point(5, 82));
        //    e.Graphics.DrawString("Corruption: " + _thisCity.Corruption.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(346, 83), sf1);
        //    e.Graphics.DrawString("Corruption: " + _thisCity.Corruption.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(227, 83, 15)), new Point(345, 82), sf1);

        //    //Draw tax/lux/sci strings
        //    e.Graphics.DrawString("50% Tax: " + _thisCity.Tax.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(6, 164));
        //    e.Graphics.DrawString("50% Tax: " + _thisCity.Tax.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(239, 159, 7)), new Point(5, 163));
        //    e.Graphics.DrawString("0% Lux: " + _thisCity.Lux.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(180, 164), sf2);
        //    e.Graphics.DrawString("0% Lux: " + _thisCity.Lux.ToString(), new Font("Arial", 14), new SolidBrush(Color.White), new Point(179, 163), sf2);
        //    e.Graphics.DrawString("50% Sci: " + _thisCity.Science.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(346, 164), sf1);
        //    e.Graphics.DrawString("50% Sci: " + _thisCity.Science.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(63, 187, 199)), new Point(345, 163), sf1);

        //    //Support + production icons
        //    e.Graphics.DrawString("Support: " + _thisCity.Support.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(6, 224));
        //    e.Graphics.DrawString("Support: " + _thisCity.Support.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(63, 79, 167)), new Point(5, 223));
        //    e.Graphics.DrawString("Production: " + _thisCity.Production.ToString(), new Font("Arial", 14), new SolidBrush(Color.Black), new Point(346, 224), sf1);
        //    e.Graphics.DrawString("Production: " + _thisCity.Production.ToString(), new Font("Arial", 14), new SolidBrush(Color.FromArgb(7, 11, 103)), new Point(345, 223), sf1);

        //    //Draw icons
        //    //e.Graphics.DrawImage(Draw.DrawCityIcons(_thisCity, 5, -2, 5, 3, 7, 2, 6, 5, 3), new Point(7, 42));

        //    sf1.Dispose();
        //    sf2.Dispose();
        //}

        //private void UnitsFromCity_Paint(object sender, PaintEventArgs e)
        //{
        //    int count = 0;
        //    int row = 0;
        //    int col = 0;
        //    double resize_factor = 1;  //orignal images are 0.67 of original, because of 50% scaling it is 0.67*1.5=1
        //    foreach (IUnit unit in Game.GetUnits.Where(n => n.HomeCity == _thisCity))
        //    {
        //        col = count % 5;
        //        row = count / 5;
        //        //e.Graphics.DrawImage(Draw.DrawUnit(unit, false, resize_factor), new Point((int)(64 * resize_factor * col), (int)(48 * resize_factor * row)));
        //        count++;

        //        if (count >= 10) { break; }
        //    }
        //}

        //private void UnitsInCity_Paint(object sender, PaintEventArgs e)
        //{
        //    StringFormat sf = new StringFormat();
        //    sf.Alignment = StringAlignment.Center;

        //    int count = 0;
        //    int row = 0;
        //    int col = 0;
        //    double resize_factor = 1.125;  //orignal images are 25% smaller, because of 50% scaling it is 0.75*1.5=1.125
        //    foreach (IUnit unit in Game.GetUnits.Where(unit => unit.X == _thisCity.X && unit.Y == _thisCity.Y))
        //    {
        //        col = count % 5;
        //        row = count / 5;
        //        //e.Graphics.DrawImage(Draw.DrawUnit(unit, false, resize_factor), new Point((int)(64 * resize_factor * col), (int)(48 * resize_factor * row) + 5 * row));
        //        e.Graphics.DrawString(_thisCity.Name.Substring(0, 3), new Font("Arial", 12), new SolidBrush(Color.Black), new Point((int)(64 * resize_factor * col) + (int)(64 * resize_factor / 2), (int)(48 * resize_factor * row) + 5 * row + (int)(48 * resize_factor)), sf);
        //        count++;
        //    }
        //    sf.Dispose();
        //}

        //private void ImprovementsList_Paint(object sender, PaintEventArgs e)
        //{
        //    //Draw city improvements
        //    int x = 12;
        //    int y = 460;
        //    int starting = ImprovementsBar.Value;   //starting improvement to draw (changes with slider)
        //    for (int i = 0; i < 9; i++)
        //    {
        //        if ((i + starting) >= (_thisCity.Improvements.Count())) { break; }  //break if no of improvements+wonders to small

        //        //draw improvements
        //        //e.Graphics.DrawImage(Images.ImprovementsSmall[(int)ThisCity.Improvements[i + starting].Type], new Point(x, y + 15 * i + 2 * i));
        //        //if ((int)ThisCity.Improvements[i + starting].Type < 39) //wonders don't have a sell icon
        //        //{
        //        //    e.Graphics.DrawImage(Images.SellIconLarge, new Point(x + 220, y + 15 * i + 2 * i - 2));
        //        //}
        //        //e.Graphics.DrawString(ThisCity.Improvements[i + starting].Name, new Font("Arial", 13), new SolidBrush(Color.Black), new Point(x + 36, y + 15 * i + 2 * i - 3));
        //        //e.Graphics.DrawString(ThisCity.Improvements[i + starting].Name, new Font("Arial", 13), new SolidBrush(Color.White), new Point(x + 35, y + 15 * i + 2 * i - 3));
        //    }
        //}

        //private void FoodStorage_Paint(object sender, PaintEventArgs e)
        //{
        //    //e.Graphics.DrawImage(Draw.DrawFoodStorage(ThisCity), new Point(0, 0));
        //}

        //private void ProductionPanel_Paint(object sender, PaintEventArgs e)
        //{
        //    //Show item currently in production (ProductionItem=0...61 are units, 62...127 are improvements)
        //    //Units are scaled by 1.15 compared to original, improvements are size 54x30
        //    if (_thisCity.ItemInProduction < 62)    //units
        //    {
        //        e.Graphics.DrawImage(ModifyImage.ResizeImage(Images.Units[_thisCity.ItemInProduction], 1), new Point(106, 7));   // Should it be zoom=1??
        //    }
        //    else    //improvements
        //    {
        //        StringFormat sf = new StringFormat();
        //        sf.Alignment = StringAlignment.Center;
        //        e.Graphics.DrawString(Game.Rules.ImprovementName[_thisCity.ItemInProduction - 62 + 1], new Font("Arial", 14), new SolidBrush(Color.Black), 146 + 1, 3 + 1, sf);
        //        e.Graphics.DrawString(Game.Rules.ImprovementName[_thisCity.ItemInProduction - 62 + 1], new Font("Arial", 14), new SolidBrush(Color.FromArgb(63, 79, 167)), 146, 3, sf);
        //        //e.Graphics.DrawImage(Images.ImprovementsLarge[ThisCity.ItemInProduction - 62 + 1], new Point(119, 28));
        //        sf.Dispose();
        //    }

        //    //e.Graphics.DrawImage(Draw.DrawCityProduction(ThisCity), new Point(0, 0));  //draw production shields and sqare around them
        //}

        //private void ImprovementsPanel_Paint(object sender, PaintEventArgs e)
        //{
        //}

        //private void BuyButton_Click(object sender, EventArgs e)
        //{
        //    //Use this so the form returns a chosen value (what it has chosen to produce)
        //    using (var CityBuyForm = new _CityBuyForm(_thisCity))
        //    {
        //        CityBuyForm.Load += new EventHandler(CityBuyForm_Load);   //so you set the correct size of form
        //        var result = CityBuyForm.ShowDialog();
        //        if (result == DialogResult.OK)  //buying item activated
        //        {
        //            int cost = 0;
        //            if (_thisCity.ItemInProduction < 62) cost = Game.Rules.UnitCost[_thisCity.ItemInProduction];
        //            else cost = Game.Rules.ImprovementCost[_thisCity.ItemInProduction - 62 + 1];
        //            Game.GetCivs[1].Money -= 10 * cost - _thisCity.ShieldsProgress;
        //            _thisCity.ShieldsProgress = 10 * cost;
        //            ProductionPanel.Refresh();
        //        }
        //    }
        //}

        //private void CityBuyForm_Load(object sender, EventArgs e)
        //{
        //    Form frm = sender as Form;
        //    frm.Location = new Point(250, 300);
        //    frm.Width = 758;
        //    frm.Height = 212;
        //}

        //private void ChangeButton_Click(object sender, EventArgs e)
        //{
        //    //Use this so the form returns a chosen value (what it has chosen to produce)
        //    using (var CityChangeForm = new _CityChangeForm(_thisCity))
        //    {
        //        CityChangeForm.Load += new EventHandler(CityChangeForm_Load);   //so you set the correct size of form
        //        var result = CityChangeForm.ShowDialog();
        //        if (result == DialogResult.OK)  //when form is closed
        //        {
        //            ProductionPanel.Refresh();
        //        }
        //    }
        //}

        //private void CityChangeForm_Load(object sender, EventArgs e)
        //{
        //    Form frm = sender as Form;
        //    frm.Width = 686;
        //    frm.Height = 458;
        //    frm.Location = new Point(200, 100);
        //}

        //private void InfoButton_Click(object sender, EventArgs e)
        //{
        //}

        //private void MapButton_Click(object sender, EventArgs e)
        //{
        //}

        //private void RenameButton_Click(object sender, EventArgs e)
        //{
        //    _CityRenameForm CityRenameForm = new _CityRenameForm(_thisCity);
        //    CityRenameForm.RefreshCityForm += RefreshThis;
        //    CityRenameForm.ShowDialog();
        //}

        //void RefreshThis()
        //{
        //    Refresh();
        //}

        //private void HappyButton_Click(object sender, EventArgs e) { }

        //private void ViewButton_Click(object sender, EventArgs e) { }

        //private void NextCityButton_Click(object sender, EventArgs e) { }

        //private void PrevCityButton_Click(object sender, EventArgs e) { }

        //private void NextCityButton_Paint(object sender, PaintEventArgs e)
        //{
        //    //Draw lines in button
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 30, 1);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 2, 29, 2);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 1, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 2, 1, 2, 32);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 1, 34, 30, 34);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 2, 33, 30, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 29, 2, 29, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 30, 1, 30, 33);
        //    //Draw the arrow icon
        //    e.Graphics.DrawImage(Images.NextCityLarge, 2, 1);
        //}

        //private void PrevCityButton_Paint(object sender, PaintEventArgs e)
        //{
        //    //Draw lines in button
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 30, 1);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 2, 29, 2);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 1, 1, 1, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(175, 175, 175)), 2, 1, 2, 32);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 1, 34, 30, 34);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 2, 33, 30, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 29, 2, 29, 33);
        //    e.Graphics.DrawLine(new Pen(Color.FromArgb(43, 43, 43)), 30, 1, 30, 33);
        //    //Draw the arrow icon
        //    e.Graphics.DrawImage(Images.PrevCityLarge, 2, 1);
        //}

        //private void ExitButton_Click(object sender, EventArgs e)
        //{
        //    //close panel???
        //}
    }
}
