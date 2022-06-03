using System.Reflection;
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
/*
string methodName = "NextDouble";
Random rand = new Random();
System.Reflection.MethodInfo info =
rand.GetType().GetMethod(methodName);
double r = (double) info.Invoke(rand, null);*/
Raylib.SetExitKey(0);
//Raylib.SetTargetFPS(60);
ScriptEngine.LoadScript("assets/test.jgui");
while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(ScriptEngine.BGCol);
    ScriptEngine.ParseStyle();
    
    foreach (var item in ScriptEngine.DynamicStyles)
    {
        Draw.DynamicGUIElement(item.Value);
    }
    Raylib.DrawFPS(10,10);
    Raylib.EndDrawing();
}
Raylib.CloseWindow();
foreach (var f in ScriptEngine.Fonts) Raylib.UnloadFont(f.Value);