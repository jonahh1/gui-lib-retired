Raylib.SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
Raylib.InitWindow(800, 450, "UI Lib");
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
ScriptEngine.LoadStyle("assets/test.jgui");


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
foreach (var f in ScriptEngine.Fonts) Raylib.UnloadFont(f.Value);