using Raylib_cs;
namespace GUI_Lib
{
    public enum AnchorType {
        top_left,
        top_center,
        top_right,

        middle_left,
        middle_center,
        middle_right,
        
        bottom_left,
        bottom_center,
        bottom_right,
    }
    static class GUITools
    {
        public static Color HexToRGB(string hex) {
            if (hex.Length < 6) return new Color(255,0,255,255);//throw new ArgumentException("Parameter cannot have a length less than six");
            int usedHash = 0;
            if (hex[0] == '#') usedHash++;
            //                                     just know that i hate this solution to join two chars    reason: that sentance was shorter than the solution
            int r =                      int.Parse(new string(new char[]{hex[usedHash],  hex[usedHash+1]}), System.Globalization.NumberStyles.HexNumber);
            int g =                      int.Parse(new string(new char[]{hex[usedHash+2],hex[usedHash+3]}), System.Globalization.NumberStyles.HexNumber);
            int b =                      int.Parse(new string(new char[]{hex[usedHash+4],hex[usedHash+5]}), System.Globalization.NumberStyles.HexNumber);
            int a = hex.Length<usedHash+7 ? 255 : int.Parse(new string(new char[]{hex[usedHash+6],hex[usedHash+7]}), System.Globalization.NumberStyles.HexNumber);

            return new Color(r,g,b,a);
        }
        public static string RGBToHex(Color col) {
            return "#"
            + col.r.ToString("X").ToLower()
            + col.g.ToString("X").ToLower()
            + col.b.ToString("X").ToLower()
            + (col.a==255?"":col.b.ToString("X").ToLower());
        }
        private static Font font;
        public static void SetFont(Font font)
        {
            GUITools.font = font;
        }
        public static float TextWidth(string text, float size) {
            return Raylib.MeasureTextEx(font, text, size, 0).X;
        }
        public static float TextHeight(string text, float size) {
            return Raylib.MeasureTextEx(font, text, size, 0).Y;
        }
        public static Vector2 TextWidthAndHeight(string text, float size) {
            return Raylib.MeasureTextEx(font, text, size, 0);
        }
        public static bool InvsibleButtonHover(Rectangle rect, MouseButton mb = MouseButton.MOUSE_BUTTON_LEFT){
            return Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect);
        }
        public static bool InvsibleButtonClick(Rectangle rect, MouseButton mb = MouseButton.MOUSE_BUTTON_LEFT){
            return Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect) && Raylib.IsMouseButtonPressed(mb);
        }
        public static Rectangle AddRects(Rectangle a, Rectangle b) {
            return new Rectangle(a.x+b.x, a.y+b.y, a.width+b.width, a.height+b.height);
        }

        public static AnchorType StringToAnchor(string val)
        {
            switch (val)
            {
                case"top-center":case"top_center":case "tc": return AnchorType.top_center;
                case"top-right":case"top_right":case "tr": return AnchorType.top_right;
                
                case"middle-left":case"middle_left":case "ml": return AnchorType.middle_left;
                case"middle-center":case"middle_center":case "mc": return AnchorType.middle_center;
                case"middle-right":case"middle_right":case "mr": return AnchorType.middle_right;

                case"bottom-left":case"bottom_left":case "bl": return AnchorType.bottom_left;
                case"bottom-center":case"bottom_center":case "bc": return AnchorType.bottom_center;
                case"bottom-right":case"bottom_right":case "br": return AnchorType.bottom_right;
                default: return AnchorType.top_left;
            }
        }
        public static Rectangle StringToRect(string val)
        {
            string[] strValues = Regex.Replace(val,@"\(|\)","").Split(",").ToArray();
            float[] values = new float[4]; //Array.ConvertAll(i, float.Parse);
            for (int i = 0; i < strValues.Length; i++)
            {
                string v = strValues[i];
                if (v == "sw") v = Raylib.GetScreenWidth().ToString();
                if (v == "sh") v = Raylib.GetScreenHeight().ToString();
                values[i] = Eval(v);
            }
            if (values.Length!=4) return new Rectangle(10,10,100, 100);
            return new Rectangle(values[0],values[1],values[2],values[3]);
        }
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

        
        public static Rectangle ModRectFromParentAndAnchor(Rectangle parent, AnchorType anchor, Rectangle rect)
        {
            float x = 0;
            float y = 0;
            switch (anchor)
            {
                case AnchorType.top_left:
                    x = parent.x;
                    y = parent.y;
                    break;
                case AnchorType.top_center:
                    x = parent.x + (parent.width - rect.width)/2;
                    y = parent.y;
                    break;
                case AnchorType.top_right: 
                    x = (parent.x + parent.width) - rect.width;
                    y = parent.y;
                    break;

                case AnchorType.middle_left:
                    x = parent.x;
                    y = parent.y + (parent.height - rect.height)/2;
                    break;
                case AnchorType.middle_center:
                    x = parent.x + (parent.width - rect.width)/2;
                    y = parent.y + (parent.height - rect.height)/2;
                    break;
                case AnchorType.middle_right:
                    x = (parent.x + parent.width) - rect.width;
                    y = parent.y + (parent.height - rect.height)/2;
                    break;

                case AnchorType.bottom_left:
                    x = parent.x;
                    y = parent.y + (parent.height - rect.height);
                    break;
                case AnchorType.bottom_center:
                    x = parent.x + (parent.width - rect.width)/2;
                    y = parent.y + (parent.height - rect.height);
                    break;
                case AnchorType.bottom_right:
                    x = (parent.x + parent.width) - rect.width;
                    y = parent.y + (parent.height - rect.height);
                    break;
                
                default: x = parent.x; y = parent.y; break;
            }
            return new Rectangle(x + rect.x, y + rect.y, rect.width, rect.height);
        }
    }
}
