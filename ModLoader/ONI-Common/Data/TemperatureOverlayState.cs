namespace ONI_Common.Data
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using UnityEngine;

    [SuppressMessage("ReSharper", "StyleCop.SA1600")]
    public class TemperatureOverlayState
    {

        // public float AbsoluteZero { get; set; } = 0;
        // public float Molten       { get; set; } = 1800f;
        // public float Scorching { get; set; } = 0.1f;
        // public float Hot    { get; set; } = 323f;
        // public float Warm      { get; set; } = 293f;
        // public float Temperate     { get; set; } = 273f;
        // public float Chilled      { get; set; } = 0.2f;
        // public float Cold { get; set; } = 0.1f;
        // public float AbsoluteZero      { get; set; } = 0;

    //     Edited color scheme
         public Color AbsoluteZeroColor = new Color32(25,  102, 255, 192);
         public Color ColdColor         = new Color32(25,  179, 255, 192);
         public Color ChilledColor      = new Color32(25,  255, 255, 192);
         public Color TemperateColor    = new Color32(25, 255, 25, 192);
         public Color WarmColor         = new Color32(236, 255, 25,  192);
         public Color HotColor          = new Color32(255, 163, 25,  192);
         public Color ScorchingColor    = new Color32(255, 83,  25,  192);
         public Color MoltenColor       = new Color32(255, 25,  25,  192);

        public float AbsoluteZero { get; set; } = 0;

        public float Cold { get; set; } = 273.15f;

        public float Chilled { get; set; } = 283.15f;

        public float Temperate { get; set; } = 293.15f;

        public float Warm { get; set; } = 303.15f;

        public float Hot { get; set; } = 310.15f;

        public float Scorching { get; set; } = 373.15f;

        public List<Color> Colors =>
        new List<Color>
        {
        this.AbsoluteZeroColor,
        this.ColdColor,
        this.ChilledColor,
        this.TemperateColor,
        this.WarmColor,
        this.HotColor,
        this.ScorchingColor,
        this.MoltenColor
        };

        public bool CustomRangesEnabled { get; set; } = true;


        public bool LogThresholds { get; set; } = false;

        public float Molten { get; set; } = 2073.15f;



        public List<float> Temperatures =>
        new List<float>
        {
        this.AbsoluteZero,
        this.Cold,
        this.Chilled,
        this.Temperate,
        this.Warm,
        this.Hot,
        this.Scorching,
        this.Molten
        };

    }
}