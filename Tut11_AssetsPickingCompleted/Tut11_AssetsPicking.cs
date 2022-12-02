using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Engine.Core.Effects;
using Fusee.Math.Core;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut11_AssetsPicking", Description = "Yet another FUSEE App.")]
    public class Tut11_AssetsPicking : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private SceneRayCaster _sceneRayCaster;
        private Transform _camTransform;
        private Transform _rightRearTransform;
        private RayCastResult _currentPick;
        private float4 _oldColor;
        


        // Init is called on startup. 
        public override void Init()
        {
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);
        }

        public override async Task InitAsync()
        {
            _scene = await AssetStorage.GetAsync<SceneContainer>("CubeCar.fus");
            _camTransform = new Transform{
                Translation = new float3(0, 5, -40),
            };
            SceneNode cam = new SceneNode
            {
                Name = "Camera",
                Components =
                { 
                    _camTransform,
                    new Camera(ProjectionMethod.Perspective, 5, 500, M.PiOver4) 
                    {
                        BackgroundColor =  (float4) ColorUint.Greenery,
                    }
                },
            };

            _scene.Children.Add(cam);

            _rightRearTransform = _scene.Children.FindNodes(node => node.Name == "RightRearWheel")?.FirstOrDefault()?.GetTransform();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
            _sceneRayCaster = new SceneRayCaster(_scene);

            await base.InitAsync();
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            _rightRearTransform.Rotation = new float3(M.MinAngle(TimeSinceStart), 0, 0);

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            _camTransform.RotateAround(float3.Zero, new float3(0, Keyboard.LeftRightAxis * DeltaTime, 0));

            if (Mouse.LeftButton)
            {
                float2 pickPos = Mouse.Position;

                RayCastResult newPick = _sceneRayCaster.RayPick(RC, pickPos).OrderBy(rr => rr.DistanceFromOrigin).FirstOrDefault();

                if (newPick?.Node != _currentPick?.Node)
                {
                    if (_currentPick != null)
                    {
                        var ef = _currentPick.Node.GetComponent<SurfaceEffect>();
                        ef.SurfaceInput.Albedo = _oldColor;
                    }
                    if (newPick != null)
                    {
                        var ef = newPick.Node.GetComponent<SurfaceEffect>();
                        _oldColor = ef.SurfaceInput.Albedo;
                        ef.SurfaceInput.Albedo = (float4) ColorUint.OrangeRed;
                    }
                    _currentPick = newPick;
                }
            }

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }     
    }
}