namespace GUI_Lib
{
    public static class Draw
    {
        public static void Rect(float x, float y, float w, float h, string c){
            Raylib.DrawRectangle((int)x, (int)y, (int)w, (int)h, GUITools.HexToRGB(c));
        }
        public static void Rect(Rectangle r, string c) {
            Raylib.DrawRectangleRec(r, GUITools.HexToRGB(c));
        }

        public static void RectLines(float x, float y, float w, float h, float s, string c){
            Draw.RectLines(new Rectangle(x,y,w,h), s, c);
        }
        public static void RectLines(Rectangle r, float s, string c) {
            Raylib.DrawRectangleLinesEx(r, s, GUITools.HexToRGB(c));
        }

        public static void Text(string text, float x, float y, Color c, float fontSize, float fontSpacing=0) {
            Raylib.DrawTextEx(Raylib.GetFontDefault(), text, new Vector2(x, y), (int)fontSize, fontSpacing, c);
        }
        public static void Text(string text, Rectangle r, AnchorType a, Color c, float fontSize, float fontSpacing=0) {
            Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, fontSpacing);
            Rectangle rect = GUITools.ModRectFromParentAndAnchor(r, a, new Rectangle(0,0,textSize.X, textSize.Y));
            Draw.Text(text, rect.x, rect.y, c, fontSize, fontSpacing);
        }


        #region styled functions
        public static void GUIRect(string name)
        {
            if (!ScriptEngine.Styles.ContainsKey(name)) return;
            StyleContainer s = ScriptEngine.Styles[name]; // style
            Raylib.DrawRectangleRec(GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, s.rect), s.backgroundCol);
            Raylib.DrawRectangleLinesEx(GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, s.rect), s.borderWidth, s.borderCol);

            if (s.text != "")
            {
                Vector2 textSize = Raylib.MeasureTextEx(Raylib.GetFontDefault(), s.text, s.fontSize, s.fontSpacing);
                Rectangle rect = GUITools.ModRectFromParentAndAnchor(s.rect, s.textAlign, new Rectangle(0,0,textSize.X, textSize.Y));
                rect = GUITools.AddRects(rect, GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, new Rectangle(0,0,s.rect.width,s.rect.height)));
                Draw.Text(s.text, rect.x, rect.y, s.foregroundCol, s.fontSize, s.fontSpacing);
            }
        }
        #endregion
    }
}
