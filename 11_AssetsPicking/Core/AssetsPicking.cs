using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;

namespace Fusee.Tutorial.Core
{
    public class AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;        
        private float _camAngle = 0;
        private TransformComponent _baseTransform;

        private TransformComponent _radhintenreTransform;
        private TransformComponent _radhintenliTransform;
        private TransformComponent _radvornereTransform;
        private TransformComponent _radvorneliTransform;
        private TransformComponent _rumpfTransform;
        private TransformComponent _AchsehintenTransform;
        private TransformComponent _AchsevorneTransform;
        private TransformComponent _TowerTransform;
        private TransformComponent _laufTransform;
        private ScenePicker _scenePicker;
        private PickResult _currentPick;
        private float3 _oldColor;

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // MATERIAL COMPONENT
                            new MaterialComponent
                            {
                                Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                                Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                            },

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = AssetStorage.Get<SceneContainer>("tankfinal.fus");

            _radhintenreTransform = _scene.Children.FindNodes(node => node.Name == "radhintenre")?.FirstOrDefault()?.GetTransform();
            _radhintenliTransform = _scene.Children.FindNodes(node => node.Name == "radhintenli")?.FirstOrDefault()?.GetTransform();
            _radvornereTransform = _scene.Children.FindNodes(node => node.Name == "radvornere")?.FirstOrDefault()?.GetTransform();
            _radvorneliTransform = _scene.Children.FindNodes(node => node.Name == "radvorneli")?.FirstOrDefault()?.GetTransform();
            _rumpfTransform = _scene.Children.FindNodes(node => node.Name == "rumpf")?.FirstOrDefault()?.GetTransform();
            _AchsehintenTransform = _scene.Children.FindNodes(node => node.Name == "Achsehinten")?.FirstOrDefault()?.GetTransform();
            _AchsevorneTransform = _scene.Children.FindNodes(node => node.Name == "Achsevorne")?.FirstOrDefault()?.GetTransform();
            _TowerTransform = _scene.Children.FindNodes(node => node.Name == "Tower")?.FirstOrDefault()?.GetTransform();
            _laufTransform = _scene.Children.FindNodes(node => node.Name == "lauf")?.FirstOrDefault()?.GetTransform();

            _scenePicker = new ScenePicker(_scene);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            //_baseTransform.Rotation = new float3(0, M.MinAngle(TimeSinceStart), 0);

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            //Kamera Bewegen mit der Maus
            var maus = Mouse.LeftButton;
            if (maus == true)
            {
                _camAngle += Mouse.Velocity.x * 0.0001f * DeltaTime / 16 * 1000;
            }

            //"zoom"-funktion 
            float achseZ = 50;
            achseZ += Mouse.Wheel;

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, achseZ) * float4x4.CreateRotationY(_camAngle);
            //RC.View = float4x4.CreateTranslation(0, -10, achseZ) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));

            if (Mouse.RightButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                PickResult newPick = null;
                if (pickResults.Count > 0)
                {
                    pickResults.Sort((a, b) => Sign(a.ClipPos.z - b.ClipPos.z));
                    newPick = pickResults[0];
                }
                    if (newPick?.Node != _currentPick?.Node)
                    {
                        if (_currentPick != null)
                        {
                            _currentPick.Node.GetMaterial().Diffuse.Color = _oldColor;
                        }
                        if (newPick != null)
                        {
                            var mat = newPick.Node.GetMaterial();
                            _oldColor = mat.Diffuse.Color;
                            mat.Diffuse.Color = new float3(2.55f, 0.69f, 0);
                        }
                        _currentPick = newPick;
                    }               
            }

            if (_currentPick != null)
            {
                if (_currentPick.Node.Name == "radhintenli")
                {
                    float radhintenli = _radhintenliTransform.Rotation.x;
                    radhintenli -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _radhintenliTransform.Rotation = new float3(radhintenli, 0, 0);
                }

                if (_currentPick.Node.Name == "radhintenre")
                {
                    float radhintenre = _radhintenreTransform.Rotation.x;
                    radhintenre -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _radhintenreTransform.Rotation = new float3(radhintenre, 0, 0);
                }

                if (_currentPick.Node.Name == "radvorneli")
                {
                    float radvorneli = _radvorneliTransform.Rotation.x;
                    radvorneli -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _radvorneliTransform.Rotation = new float3(radvorneli, 0, 0);
                }

                if (_currentPick.Node.Name == "radvornere")
                {
                    float radvornere = _radvornereTransform.Rotation.x;
                    radvornere -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _radvornereTransform.Rotation = new float3(radvornere, 0, 0);
                }

                if (_currentPick.Node.Name == "Achsehinten")
                {
                    float achsehinten = _AchsehintenTransform.Rotation.x;
                    achsehinten -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _AchsehintenTransform.Rotation = new float3(achsehinten, 0, 0);
                }

                if (_currentPick.Node.Name == "Achsevorne")
                {
                    float achsevorne = _AchsevorneTransform.Rotation.x;
                    achsevorne -= Keyboard.WSAxis * 2.0f * (DeltaTime);
                    _AchsevorneTransform.Rotation = new float3(achsevorne, 0, 0);
                }

                if (_currentPick.Node.Name == "Tower")
                {
                    float tower = _TowerTransform.Rotation.y;
                    tower -= Keyboard.LeftRightAxis * 2.0f * (DeltaTime);
                    _TowerTransform.Rotation = new float3(0, tower, 0);
                }

                if (_currentPick.Node.Name == "lauf")
                {
                    float lauf = _laufTransform.Rotation.x;
                    if (Keyboard.GetKey(KeyCodes.Up))
                    {
                        if (lauf <= 0.2f)
                        {
                            lauf += 0.2f * (DeltaTime);
                            _laufTransform.Rotation = new float3(lauf, 0, 0);
                        }
                    }
                    if (Keyboard.GetKey(KeyCodes.Down))
                    {
                        if (lauf >= -0.2f)
                        {
                            lauf -= 0.2f * (DeltaTime);
                            _laufTransform.Rotation = new float3(lauf, 0, 0);
                        }
                    }
                                        
                }


            }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}
