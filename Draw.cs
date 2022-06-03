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

        public static void Text(string text, float x, float y, Color c, Font font, float fontSize, float fontSpacing=0) {
            Raylib.DrawTextEx(font, text, new Vector2(x, y), (int)fontSize, fontSpacing, c);
        }
        public static void Text(string text, Rectangle r, AnchorType a, Color c, Font font, float fontSize, float fontSpacing=0) {
            Vector2 textSize = Raylib.MeasureTextEx(font, text, fontSize, fontSpacing);
            Rectangle rect = GUITools.ModRectFromParentAndAnchor(r, a, new Rectangle(0,0,textSize.X, textSize.Y));
            Draw.Text(text, rect.x, rect.y, c, font, fontSize, fontSpacing);
        }

        #region styled functions
        public static void StaticGUIElement(string name)
        {
            if (!ScriptEngine.StaticStyles.ContainsKey(name)) return;
            StaticGUIElement(ScriptEngine.StaticStyles[name]);
        }
        public static void StaticGUIElement(StaticStyle s)
        {
            Raylib.DrawRectangleRec(GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, s.rect), s.backgroundCol);
            Raylib.DrawRectangleLinesEx(GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, s.rect), s.borderWidth, s.borderCol);

            if (s.text != "")
            {
                Vector2 textSize = Raylib.MeasureTextEx(s.font, s.text, s.fontSize, s.fontSpacing);
                Rectangle rect = GUITools.ModRectFromParentAndAnchor(s.rect, s.textAlign, new Rectangle(0,0,textSize.X, textSize.Y));
                rect = GUITools.AddRects(rect, GUITools.ModRectFromParentAndAnchor(s.parent, s.anchor, new Rectangle(0,0,s.rect.width,s.rect.height)));
                Draw.Text(s.text, rect.x, rect.y, s.foregroundCol, s.font, s.fontSize, s.fontSpacing);
            }
            foreach (MethodInfo func in s.functions)
            {
                func.Invoke(null,null);
            }
        }
        public static void DynamicGUIElement(DynamicStyle s) // s = style
        {
            if (s.styles.Count>0)
            {
                if (s.IsMousePressed(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    var styles = s.styles.Where(ss=>ss.Key.Contains("leftclick")).ToList();
                    if (styles.Count() > 0)
                    {
                        StaticGUIElement(styles.First().Value);
                        return;
                    }
                    else goto defaultStyle;
                }
                else if (s.IsMouseDown(MouseButton.MOUSE_BUTTON_LEFT))
                {
                    var styles = s.styles.Where(ss=>ss.Key.Contains("lefthold")).ToList();
                    if (styles.Count() > 0)
                    {
                        StaticGUIElement(styles.First().Value);
                        return;
                    }
                    else goto defaultStyle;
                }
                else if (s.IsMousePressed(MouseButton.MOUSE_BUTTON_RIGHT))
                {
                    var styles = s.styles.Where(ss=>ss.Key.Contains("rightclick")).ToList();
                    if (styles.Count() > 0)
                    {
                        StaticGUIElement(styles.First().Value);
                        return;
                    }
                    else goto defaultStyle;
                }
                else if (s.IsMouseDown(MouseButton.MOUSE_BUTTON_RIGHT))
                {
                    var styles = s.styles.Where(ss=>ss.Key.Contains("righthold")).ToList();
                    if (styles.Count() > 0)
                    {
                        StaticGUIElement(styles.First().Value);
                        return;
                    }
                    else goto defaultStyle;
                }
                else if (s.IsMouseHovering())
                {
                    var styles = s.styles.Where(ss=>ss.Key.Contains("hover")).ToList();
                    if (styles.Count() > 0)
                    {
                        StaticGUIElement(styles.First().Value);
                        return;
                    }
                    else goto defaultStyle;
                }
                else goto defaultStyle;
            }
            
            defaultStyle:
            StaticGUIElement(s.baseStyle);
        }
        #endregion
    }
}
