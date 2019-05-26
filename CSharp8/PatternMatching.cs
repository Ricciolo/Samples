using System;

namespace Demo
{
    class PatternProgram
    {
        public PatternProgram()
        {
            Weather w = new Weather
            {
                Temperature = 22,
                Sky = Sky.Cloudy,
                Wind = Wind.Weak
            };

            Console.WriteLine(Sample5(w));
        }

        static string Sample0(Weather w)
        {
            switch (w.Sky)
            {
                case Sky.Cloudy:
                    return "Nuvoloso";
                case Sky.Sunny:
                    return "Soleggiato";
                case Sky.Rainy:
                    return "Piovoso";
                default:
                    return "Indefinito";
            }
        }

        static string Sample1(Weather w) =>
            w.Sky switch
            {
                Sky.Cloudy => "Nuvoloso",
                Sky.Sunny => "Soleggiato",
                Sky.Rainy => "Piovoso",
                _ => "Indefinito"
            };

        static string Sample2(Weather w) =>
                 w switch
                 {
                     { Sky: Sky.Cloudy } => "Nuvoloso",
                     { Sky: Sky.Sunny } => "Soleggiato",
                     { Sky: Sky.Rainy, Wind: Wind.Strong } => "Burrasca",
                     { Sky: Sky.Rainy } => "Piovoso",
                     null => "Sconosciuto",
                     _ => "Indefinito"
                 };

        static string Sample3(Weather w) =>
                (w.Sky, w.Wind) switch
                {
                    (Sky.Cloudy, _) => "Nuvoloso",
                    (Sky.Sunny, _) => "Soleggiato",
                    (Sky.Rainy, Wind.Strong) => "Burrasca",
                    (Sky.Rainy, _) => "Piovoso",
                    (_, _) => "Sconosciuto",
                };

        static string Sample4(Weather w) =>
                w switch
                {
                    (Sky.Cloudy, _, _) => "Nuvoloso",
                    (Sky.Sunny, _, _) => "Soleggiato",
                    (Sky.Rainy, Wind.Strong, _) => "Burrasche",
                    (Sky.Rainy, _, _) => "Piovoso",
                    (_, _, float t) => $"Temperatura {t:0.0}",
                };

        static string Sample5(Weather w) =>
                w switch
                {
                    { Sky: Sky.Cloudy } => w.Wind switch {
                        Wind.Strong => "Nuvoloso con vento forte",
                        Wind.Weak => "Nuvoloso con vento debole",
                        _ => "Nuvoloso",
                    },
                    { Sky: Sky.Sunny } => "Soleggiato",
                    { Sky: Sky.Rainy, Wind: Wind.Strong } => "Burrasca",
                    { Sky: Sky.Rainy } => "Piovoso",
                    null => "Sconosciuto",
                    _ => "Indefinito"
                };
    }

    public class Weather
    {
        public Wind Wind { get; set; }

        public Sky Sky { get; set; }

        public float Temperature { get; set; }

        public void Deconstruct(out Sky sky, out Wind wind, out float temperature)
        {
            sky = Sky;
            wind = Wind;
            temperature = Temperature;
        }
    }

    public enum Wind
    {
        None,
        Weak,
        Strong
    }

    public enum Sky
    {
        Sunny,
        Cloudy,
        Rainy
    }
}
