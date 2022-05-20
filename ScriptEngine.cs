namespace GUI_Lib
{
    public class StyleContainer {
        public Color backgroundCol;
        public Color foregroundCol;
        
        public Color borderCol;
        public float borderWidth;

        public Rectangle rect;
        public Rectangle parent;
        public AnchorType anchor;

        public string text;
        public AnchorType textAlign;
        public float fontSize;
        public float fontSpacing; public void SetFontSpacing(float v) {fontSpacing = v; Console.WriteLine(fontSpacing);}
        //public Font font;

        public StyleContainer()
        {
            backgroundCol = Color.GRAY;
            foregroundCol = Color.BLACK;
            borderCol = Color.WHITE;

            borderWidth = 3;

            rect = new Rectangle(0,0,100,100);
            parent = new Rectangle(0,0,Raylib.GetScreenWidth(),Raylib.GetScreenHeight());
            anchor = AnchorType.top_left;

            text = "";
            textAlign = AnchorType.middle_center;
            fontSize = 20;
            fontSpacing = 1;
            //font = Raylib.LoadFont("assets");

        }
    }
    public static class ScriptEngine
    {
        public static Dictionary<string, string> Variables = new Dictionary<string, string>();
        public static Dictionary<string, StyleContainer> Styles = new Dictionary<string, StyleContainer>();
        public static Color BGCol = Color.WHITE;
        public static float Eval(string expression)
        {
            return (float)(double)new System.Xml.XPath.XPathDocument
            (new StringReader("<r/>")).CreateNavigator().Evaluate
            (string.Format("number({0})", new
            System.Text.RegularExpressions.Regex(@"([\+\-\*])")
            .Replace(expression, " ${1} ")
            .Replace("/", " div ")
            .Replace("%", " mod ")));
        }
        public static string GetVariable(string key)
        {
            var I = Variables;
            return Variables[key.Remove(0,1)];
        }
        static char cmdPrefix = '@';
        static char varPrefix = '$';
        static string spaceMatch = @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+";
        public static string[] formatCode(string src)
        {
            return Regex
                .Split(src, @"(?<!\^);(?![^\{\}]*\})")
                .Where(l => l.Length>0)
                .Select(l => l =
                    (l[0]==cmdPrefix
                        ?l
                        :Regex.Replace(l, spaceMatch, "$1")
                    ).Replace("^;", ";")
            ).ToArray();
        }

        public static void ParseStyle(string[] rawCode)
        {
            Styles.Clear();
            Variables.Clear();
            // formating the code into one statement per line
            string colapsedCode = string.Join("",rawCode);
            List<string> fromatedCode = formatCode(colapsedCode).ToList();
            
            // creating variables
            /*foreach (string line in fromatedCode)*/for (int i = 0; i < fromatedCode.Count; i++)
            {
                string line = fromatedCode[i];
                if (line[0] == cmdPrefix)
                {
                    string[] args = line.Remove(0,1).Split(" ", 3);
                    if (args.Length >= 2 && args[0] == "import")
                    {
                        string[] import = formatCode(string.Join("",File.ReadAllLines("assets/imports/" + args[1]))).Select(l => l = Regex.Replace(l, "{|}", "")).ToArray();
                        
                        fromatedCode.RemoveAt(i);
                        for (int imp = 0; imp < import.Length; imp++)
                        {
                            fromatedCode.Insert(i+imp, import[imp]);
                        }
                        args = fromatedCode[i].Remove(0,1).Split(" ", 3);
                    }
                    if (args.Length >= 3 && args[0] == "setvar")
                        Variables.Add(args[1], Regex.Replace(args[2], spaceMatch, "$1"));
                    if (args.Length >= 2 && args[0] == "setbackground") BGCol = GUITools.HexToRGB(args[1]);
                }
            }
            var ggggggg = Variables;
            // replacing varibale refrences in variables with real values
            while (Variables.Where(v => v.Value.Contains(varPrefix)).Count() > 0)
                foreach (var item in Variables)
                {
                    if (!item.Value.Contains(varPrefix)) continue;

                    Match match = Regex.Match(item.Value, string.Format(@"\{0}(\w)*", varPrefix));
                    Variables[item.Key] = item.Value.Replace(match.Value, GetVariable(match.Value));
                }
            
            // replacing varibale refrences in code with real values
            while (fromatedCode.Where(v => v.Contains(varPrefix)).Count() > 0)
                for (int i = 0; i < fromatedCode.Count; i++)
                {
                    if (!fromatedCode[i].Contains(varPrefix)) continue;

                    Match match = Regex.Match(fromatedCode[i], string.Format(@"\{0}(\w)*", varPrefix));
                    fromatedCode[i] = fromatedCode[i].Replace(match.Value, GetVariable(match.Value));
                }

            // creating style classes from the formated code
            foreach (string line in fromatedCode)
            {
                if (line[0] == cmdPrefix) continue;

                StyleContainer style = new StyleContainer();

                string[] splitLine = Regex.Split(line, @"{|;|}").Where(l=>l.Length>0).ToArray();
                if (splitLine.Length < 2) continue;

                for (int i = 1; i < splitLine.Length; i++)
                {
                    string[] v = splitLine[i].Replace(";","").Split(":");
                    if (v.Length != 2) continue;
                    
                    switch (v[0].ToLower())
                    {
                        case "background-col": style.backgroundCol = GUITools.HexToRGB(v[1]); continue;
                        case "foreground-col": style.foregroundCol = GUITools.HexToRGB(v[1]); continue;

                        case "border-col":   style.borderCol   = GUITools.HexToRGB(v[1]); continue;
                        case "border-width": style.borderWidth = Eval(v[1]);  continue;

                        case "rect":   style.rect   = GUITools.StringToRect(v[1]);   continue;
                        case "parent": style.parent = GUITools.StringToRect(v[1]);   continue;
                        case "anchor": style.anchor = GUITools.StringToAnchor(v[1]); continue;

                        case "text":         style.text        = v[1].Replace("\"", "");        continue;
                        case "text-align":   style.textAlign   = GUITools.StringToAnchor(v[1]); continue;
                        case "font-size":    style.fontSize    = Eval(v[1]);        continue;
                        case "font-spacing": style.fontSpacing = Eval(v[1]);        continue;
                        //case "font":         style.font        = GUITools.StringToAnchor(v[1]); continue;
                        default: continue;
                    }
                }
                if (!Styles.ContainsKey(splitLine[0])) Styles.Add(splitLine[0], style);
            }
        }
    }
}