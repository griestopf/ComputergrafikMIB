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
        private float _camAngle = -2.335f;
        private float _camAngleVelocity = 0;
        private PickResult _currentPick;
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
        private TransformComponent _trailerTransform;
        private ScenePicker _scenePicker;       
        private float3 _oldColor;
        private float _d = 15;
        



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

            _trailerTransform = new TransformComponent { Rotation = new float3(-M.Pi / 5.7f, 0, 0), Scale = float3.One, Translation = new float3 (0,0,-30)};
            _scene.Children.Add(new SceneNodeContainer
            {
                Components = new List<SceneComponentContainer>
                {
                    _trailerTransform,
                    new MaterialComponent { Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) }, Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }},
                    SimpleMeshes.CreateCuboid(new float3(2, 2, 2))
                }
            });
          
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

            if (_camAngleVelocity > 0)
            {
                _camAngleVelocity -= 0.03f;
                if (_camAngleVelocity < 0)
                {
                    _camAngleVelocity = 0;
                }
            };
            if (_camAngleVelocity < 0)
            {
                _camAngleVelocity += 0.03f;
                if (_camAngleVelocity > 0)
                {
                    _camAngleVelocity = 0;
                }
            };

            var maus = Mouse.LeftButton;
            if (maus == true)
            {
                _camAngle += Mouse.Velocity.x * 0.0001f * DeltaTime / 16 * 1000;
            }
            _camAngle -= _camAngleVelocity;

            //"zoom"-funktion 
            //float achseZ = 50;
            //achseZ += Mouse.Wheel;

            // Setup the camera 
            //RC.View = float4x4.CreateTranslation(0, -10, achseZ) * float4x4.CreateRotationY(_camAngle);
            //RC.View = float4x4.CreateTranslation(0, -10, achseZ) * float4x4.CreateRotationX(-(float) Atan(15.0 / 40.0));
            RC.View = float4x4.CreateRotationX(-M.Pi / 7.3f) * float4x4.CreateRotationY(M.Pi - _trailerTransform.Rotation.y) * float4x4.CreateTranslation(-_trailerTransform.Translation.x, -10, -_trailerTransform.Translation.z);
            if (Mouse.RightButton)
            {
                float2 pickPosClip = Mouse.Position * new float2(2.0f / Width, -2.0f / Height) + new float2(-1, 1);
                _scenePicker.View = RC.View;
                _scenePicker.Projection = RC.Projection;
                List<PickResult> pickResults = _scenePicker.Pick(pickPosClip).ToList();
                if (pickResults.Count > 0)
                {
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

                else
                {

                    if (_currentPick != null)
                    {
                        _currentPick.Node.GetMaterial().Diffuse.Color = _oldColor;
                        _currentPick = null;


                    }
                }
                
            }

            float lauf = _laufTransform.Rotation.x;
            if (Keyboard.GetKey(KeyCodes.Down))
            {
                if (lauf <= 0.2f)
                {
                    lauf += 0.2f * (DeltaTime);
                    _laufTransform.Rotation = new float3(lauf, 0, 0);
                }
            }
            if (Keyboard.GetKey(KeyCodes.Up))
            {
                if (lauf >= -0.2f)
                {
                    lauf -= 0.2f * (DeltaTime);
                    _laufTransform.Rotation = new float3(lauf, 0, 0);
                }
            }

            float tower = _TowerTransform.Rotation.y;
            tower += Keyboard.LeftRightAxis * 2.0f * (DeltaTime);
            _TowerTransform.Rotation = new float3(0, tower, 0);

            float xRadhlinks = _radhintenliTransform.Rotation.x;
            xRadhlinks += Keyboard.WSAxis * 0.09f * (DeltaTime / 16 * 1000);
            _radhintenliTransform.Rotation = new float3(xRadhlinks, 0, 0);

            float xRadhrechts = _radhintenreTransform.Rotation.x;
            xRadhrechts += Keyboard.WSAxis * 0.09f * (DeltaTime / 16 * 1000);
            _radhintenreTransform.Rotation = new float3(xRadhrechts, 0, 0);

            float xRadvlinks = _radvorneliTransform.Rotation.x;
            float yRadvlinks = _radvorneliTransform.Rotation.y;
            xRadvlinks += Keyboard.WSAxis * 0.09f * (DeltaTime / 16 * 1000);
            yRadvlinks = Keyboard.ADAxis * 0.35f;
            _radvorneliTransform.Rotation = new float3(xRadvlinks, yRadvlinks, 0);

            float xRadvrechts = _radvornereTransform.Rotation.x;
            float yRadvrechts = _radvornereTransform.Rotation.y;
            xRadvrechts += Keyboard.WSAxis * 0.09f * (DeltaTime / 16 * 1000);
            yRadvrechts = Keyboard.ADAxis * .35f;
            _radvornereTransform.Rotation = new float3(xRadvlinks, yRadvrechts, 0);

            float3 pAalt = _rumpfTransform.Translation;
            float3 pBalt = _trailerTransform.Translation;

            float posVel = Keyboard.WSAxis * DeltaTime;
            float rotVel = Keyboard.ADAxis * DeltaTime;

            float newRot = _rumpfTransform.Rotation.y + (rotVel * Keyboard.WSAxis * DeltaTime * 30);
            _rumpfTransform.Rotation = new float3(0, newRot, 0);

            float3 pAneu = _rumpfTransform.Translation + new float3(posVel * M.Sin(newRot) * 10, 0, posVel * M.Cos(newRot) * 10);
            _rumpfTransform.Translation = pAneu;

            float3 pBneu = pAneu + (float3.Normalize(pBalt - pAneu) * _d);
            _trailerTransform.Translation = pBneu;

            _trailerTransform.Rotation = new float3(0, (float)System.Math.Atan2(float3.Normalize(pBalt - pAneu).x, float3.Normalize(pBalt - pAneu).z), 0);

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
