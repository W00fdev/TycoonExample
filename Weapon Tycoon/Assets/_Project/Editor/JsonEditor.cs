using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class JsonEditor : EditorWindow
    {
        private string _jsonInput = "{ \"name\": \"Unity\", \"version\": 2023, \"isAwesome\": true, \"features\": [\"Scripting\", \"Physics\"] }";
        private Dictionary<string, object> parsedJson = new Dictionary<string, object>();
        private Vector2 scrollPosition;
        
        public static void ShowWindow(string jsonInput)
        {
            var window = GetWindow<JsonEditor>(true, "JSON Editor");
            window.minSize = new Vector2(300f, 300f);
            window.SetJsonInput(jsonInput);
            
            window.Show();
        }

        private void SetJsonInput(string json)
        {
            _jsonInput = json;
        }
        
        private void OnGUI()
        {
            GUILayout.Label("JSON Input", EditorStyles.boldLabel);

            _jsonInput = EditorGUILayout.TextArea(_jsonInput, GUILayout.Height(100));
            if (GUILayout.Button("Parse JSON"))
            {
                try
                {
                    parsedJson = ParseJsonToDictionary(_jsonInput);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error parsing JSON: {ex.Message}");
                    parsedJson.Clear();
                }
            }

            GUILayout.Space(10);

            // Прокручиваемая область для редактирования JSON
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            if (parsedJson.Count > 0)
            {
                GUILayout.Label("Edit JSON", EditorStyles.boldLabel);
                foreach (var key in new List<string>(parsedJson.Keys))
                {
                    DrawEditableField(key, parsedJson[key]);
                }
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            // Кнопка для сохранения изменений в JSON
            if (GUILayout.Button("Save JSON"))
            {
                _jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
            }
        }

        private void DrawEditableField(string key, object value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(key, GUILayout.Width(350));
            GUILayout.Label(value.GetType().ToString(), GUILayout.Width(250));

            if (value is string strValue)
            {
                parsedJson[key] = EditorGUILayout.TextField(strValue);
            }
            else if (value is int intValue)
            {
                parsedJson[key] = EditorGUILayout.IntField(intValue);
            }
            else if (value is long longValue)
            {
                parsedJson[key] = EditorGUILayout.LongField(longValue);
            }
            else if (value is float floatValue)
            {
                parsedJson[key] = EditorGUILayout.FloatField(floatValue);
            }
            else if (value is bool boolValue)
            {
                parsedJson[key] = EditorGUILayout.Toggle(boolValue);
            }
            else if (value is JArray jArrayValue)
            {
                GUILayout.Label("[Array]");
                if (jArrayValue.First is not null)
                {
                    JTokenType type = jArrayValue.First.Type;
                    GUILayout.Label(jArrayValue.First.Type.ToString(), GUILayout.Width(150));
                    if (GUILayout.Button("Edit Array", GUILayout.Width(100)))
                    {
                        switch (type)
                        {
                            case JTokenType.Object:
                                OpenListEditor(key, jArrayValue.ToObject<List<object>>());
                                break;
                            case JTokenType.Integer:
                                OpenListEditor(key, jArrayValue.ToObject<List<int>>());
                                break;
                            case JTokenType.Float:
                                OpenListEditor(key, jArrayValue.ToObject<List<float>>());
                                break;
                            case JTokenType.String:
                                OpenListEditor(key, jArrayValue.ToObject<List<string>>());
                                break;
                            case JTokenType.Boolean:
                                OpenListEditor(key, jArrayValue.ToObject<List<bool>>());
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }    
                }
                else
                {
                    GUILayout.Label("Null array", GUILayout.Width(150));
                }
            }
            else if (value is List<int> listInt)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listInt);
                }
            }
            else if (value is List<long> listLong)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listLong);
                }
            }
            else if (value is List<float> listFloat)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listFloat);
                }
            }
            else if (value is List<bool> listBool)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listBool);
                }
            }
            else if (value is List<string> listString)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listString);
                }
            }
            else if (value is List<object> listValue)
            {
                GUILayout.Label("[List]");
                if (GUILayout.Button("Edit List", GUILayout.Width(100)))
                {
                    OpenListEditor(key, listValue);
                }
            }
            else if (value is Dictionary<string, object> dictValue)
            {
                GUILayout.Label("{Object}");
                if (GUILayout.Button("Edit Object", GUILayout.Width(100)))
                {
                    OpenObjectEditor(key, dictValue);
                }
            }
            else
            {
                GUILayout.Label(value?.ToString() ?? "null");
            }

            GUILayout.EndHorizontal();
        }

        private void OpenArrayEditor(string key, Array arrayValue)
        {
            var editor = GetWindow<JsonArrayEditor>("Edit array");
            editor.Initialize(arrayValue, (updatedArray) => parsedJson[key] = updatedArray);
        }

        private void OpenListEditor(string key, List<object> list)
        {
            var editor = GetWindow<JsonListObjectEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }

        private void OpenListEditor(string key, List<int> list)
        {
            var editor = GetWindow<JsonListIntEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }

        private void OpenListEditor(string key, List<long> list)
        {
            var editor = GetWindow<JsonListLongEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }
        
        private void OpenListEditor(string key, List<float> list)
        {
            var editor = GetWindow<JsonListFloatEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }
        
        private void OpenListEditor(string key, List<string> list)
        {
            var editor = GetWindow<JsonListStringEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }
        
        private void OpenListEditor(string key, List<bool> list)
        {
            var editor = GetWindow<JsonListBoolEditor>("Edit List");
            editor.Initialize(list, (updatedList) => parsedJson[key] = updatedList);
        }
        
        private void OpenObjectEditor(string key, Dictionary<string, object> dict)
        {
            var editor = GetWindow<JsonObjectEditor>("Edit Object");
            editor.Initialize(dict, (updatedDict) => parsedJson[key] = updatedDict);
        }

        private Dictionary<string, object> ParseJsonToDictionary(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json, new Newtonsoft.Json.JsonSerializerSettings
            {
                Converters = { new Newtonsoft.Json.Converters.ExpandoObjectConverter() }
            });
        }
    }
}