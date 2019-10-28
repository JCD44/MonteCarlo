using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Purely meant for testing and debugging.
/// </summary>
namespace MonteCarlo.Extensions
{
    public static class Extentions
    {
        private static HashSet<Tracker> ObjectsSeen { get; set; } = new HashSet<Tracker>();

        public static List<Variance> Compare<T>(this T val1, T val2)
        {
            ObjectsSeen.Clear();
            var results = CompareDetails(val1, val2, new State() { Path = "Obj" });
            ObjectsSeen.Clear();

            return results;
        }

        private static bool IsTracked(object val, State state)
        {
            return 0 != ObjectsSeen.Where(a => a.Path.Contains(state.Path)).Where(b => Equals(b.Pointer, val)).Count();
        }

        private static void AddTracker(object val, State state)
        {
            ObjectsSeen.Add(new Tracker() { Pointer = val, Path = state.Path });
        }

        private static List<Variance> CompareDetails<T>(T val1, T val2, State state)
        {
            List<Variance> variances = new List<Variance>();

            if (state.Depth > 10) return variances;
            if (IsTracked(val1, state)) return variances;
            if (IsTracked(val2, state)) return variances;

            if (!Equals(val1, val2))
            {
                AddTracker(val1, state);
                AddTracker(val2, state);
            }
            else
            {
                // These are equal...
                AddTracker(val1, state);
                return variances;
            }

            FieldInfo[] fi = val1.GetType().GetFields();
            foreach (FieldInfo f in fi)
            {
                Variance v = new Variance
                {
                    Prop = f.Name,
                    ValA = f.GetValue(val1),
                    ValB = f.GetValue(val2),
                    Type = "Field",
                    Path = state.Path
                };
                if (!Object.Equals(v.ValA, v.ValB))
                {
                    if (IsUsefulType(f.FieldType) || v.ValA == null || v.ValB == null)
                    {
                        variances.Add(v);
                    }
                    else
                    {
                        var result = CompareDetails(v.ValA, v.ValB, new State(state, f.Name));
                        variances.AddRange(result);
                        if (result.Count == 0) variances.Add(v);
                    }
                }
            }

            var pi = val1.GetType().GetProperties().Where(a=>a.CanRead && a.GetIndexParameters().Length==0);
            foreach (PropertyInfo f in pi)
            {
                Variance v = new Variance
                {
                    Prop = f.Name,
                    ValA = f.GetValue(val1),
                    ValB = f.GetValue(val2),
                    Type = "Property",
                    Path = state.Path
                };

                if (!Object.Equals(v.ValA, v.ValB))
                {
                    if (IsUsefulType(f.PropertyType) || v.ValA == null || v.ValB == null)
                    {
                        variances.Add(v);
                    }
                    else
                    {
                        var result = CompareDetails(v.ValA, v.ValB, new State(state, f.Name));
                        variances.AddRange(result);
                        if (result.Count == 0)
                        {
                            v.Info = "No obvious diff, but returned != ";
                            variances.Add(v);
                        }
                    }
                }
            }

            return variances;
        }


        public static bool IsUsefulType(Type t)
        {
            if(t.IsEnum || t.IsPrimitive || t == typeof(string) ||
                Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }

            return false;
        }
    }

    public class State
    {
        public string Path { get; set; }
        public int Depth { get; set; } = 1;

        public State() { }
        public State(State state, string path) {
            this.Path = state.Path + "." + path;
            this.Depth = state.Depth + 1;
        }
    }


    public class Tracker
    {
        public object Pointer { get; set; }
        public string Path { get; set; }
    }

    public class Variance
    {
        public string Prop { get; set; }
        public object ValA { get; set; }
        public object ValB { get; set; }
        public string Info { get; set; } = "Diff";
        public string Type { get; set; }
        public string Path { get; set; }
        public override string ToString() => $"{Path.TrimEnd('.')}.{Prop} - {Type} had {Info} - {ValA} != {ValB}";
    }
}
