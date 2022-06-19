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
        private Transform _camTransform;
        private float4 _oldColor;
        

        // Init is called on startup. 
        public override void Init()
        {
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);
        }

        SceneContainer CreateScene()
        {
            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNode>
                {
                    new SceneNode
                    {
                        Components = new List<SceneComponent>
                        {
                            // TRANSFROM COMPONENT
                            new Transform(),

                            // SHADER EFFECT COMPONENT
                            SimpleMeshes.MakeMaterial((float4) ColorUint.LightGrey),

                            // MESH COMPONENT
                            // SimpleAssetsPickinges.CreateCuboid(new float3(10, 10, 10))
                            SimpleMeshes.CreateCuboid(new float3(10, 10, 10))
                        }
                    },
                }
            };
        }


        public override async Task InitAsync()
        {
            _scene = CreateScene();

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

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
            
            await base.InitAsync();
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            _camTransform.RotateAround(float3.Zero, new float3(0, Keyboard.LeftRightAxis * DeltaTime, 0));


            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }     
    }
}