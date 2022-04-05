
using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    static Function[] functions = { Wave, MultiWave, Ripple };

    public enum FunctionName
    {
        Wave,
        MultiWave,
        Ripple
    }
    
    public delegate Vector3 Function (float u, float v, float t);
    
    public static Function GetFunction (FunctionName name)
    {
        return functions[(int)name];
    }
    
    public static FunctionName GetNextFunctionName (FunctionName name) 
    {
        return (int)name < functions.Length - 1 ? name + 1 : 0;
    }
    
    public static FunctionName GetRandomFunctionNameOtherThan (FunctionName name) 
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }
    
    public static FunctionName GetRandomFunctionName () 
    {
        var choice = (FunctionName)Random.Range(0, functions.Length);
        return choice;
    }
    
    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;

        return p;
    }
    
    public static Vector3 MultiWave (float u, float v, float t) {
        
        Vector3 p;
        
        p.x = u;
        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(PI * 2 * (u + v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f / 2.5f;
        p.z = v;
        
        return p;
    }
    
    public static Vector3 Ripple (float u, float v, float t)
    {
        Vector3 p;
        float d = Sqrt(u * u + v * v);//Abs(u);

        p.x = u;
        p.y = Sin(PI * (4*d - t));
        p.y /= (1 + 10*d);
        p.z = v;
        
        return p;
    }
    
    public static Vector3 Sphere (float u, float v, float t) {
        Vector3 p;
        float r = Cos(0.5f * PI * v);

        p.x = r * Cos(PI * u);
        p.y = Sin(PI * 0.5f * v);
        p.z = r * Sin(PI * u);
        return p;
    }
}
