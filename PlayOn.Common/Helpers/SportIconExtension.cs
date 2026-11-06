using PlayOn.Common.Enums;

namespace PlayOn.Common.Helpers
{
    public static class SportIconExtension
    {

        private static readonly Dictionary<SportIcon, string> CssClasses = new()
        {
            [SportIcon.TrophyDefault] = "fa-solid fa-trophy",
            [SportIcon.Football] = "fa-solid fa-futbol",
            [SportIcon.Basketball] = "fa-solid fa-basketball",
            [SportIcon.TableTennisPaddle] = "fa-solid fa-table-tennis-paddle-ball",
            [SportIcon.Volleyball] = "fa-solid fa-volleyball",
            [SportIcon.Badminton] = "fa-solid fa-feather",
            [SportIcon.Handball] = "fa-solid fa-hand",
            [SportIcon.Rugby] = "fa-solid fa-football",
            [SportIcon.HockeyPuck] = "fa-solid fa-hockey-puck",
            [SportIcon.Baseball] = "fa-solid fa-baseball"
        };

        public static string ToCssClass(this SportIcon icon)
        {
            if (CssClasses.TryGetValue(icon, out var css))
            {
                return css;
            }
            else
            {
                return CssClasses[SportIcon.TrophyDefault];
            }
        }
    }
}