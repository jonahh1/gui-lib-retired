Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
Raylib.InitWindow(800, 450, "UI Lib");
//NOTE: this is BY FAR the thing i am most proud of.
/*
drawing each frame:
4:     ~1400
40:    ~1200
400:   ~600
4000:  ~80
40000: ~9
*/
Raylib.SetExitKey(0);
Raylib.SetTargetFPS(60);
ScriptEngine.Start("assets/test.jgui");

double Evaluate(string expression)
{
    return (double)new System.Xml.XPath.XPathDocument
    (new StringReader("<r/>")).CreateNavigator().Evaluate
    (string.Format("number({0})", new
    System.Text.RegularExpressions.Regex(@"([\+\-\*])")
    .Replace(expression, " ${1} ")
    .Replace("/", " div ")
    .Replace("%", " mod ")));
}

Console.WriteLine(Evaluate("500/2"));

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(ScriptEngine.BGCol);
    ScriptEngine.ParseStyle();
    foreach (var item in ScriptEngine.Styles)
    {
        Draw.GUIRect(item.Key);
    }
    Raylib.DrawFPS(10,10);
    Raylib.EndDrawing();
}
Raylib.CloseWindow();