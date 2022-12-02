using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Engine.Core.Effects;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.Gui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut09_HierarchyAndInput", Description = "Yet another FUSEE App.")]
    public class Tut09_HierarchyAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _baseTransform;


       SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = 
                {
                    new SceneNode 
                    {
                        Name = "Camera",
                        Components = 
                        {
                            new Transform
                            {
                                Translation = new float3(0, 10, -50),
                            },
                            new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4) 
                            {
                                BackgroundColor =  (float4) ColorUint.Greenery
                            }
                        }
                    },

                    new SceneNode
                    {
                        Name = "Robot",
                        Components = 
                        {
                            // TRANSFORM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    }
                }
            };
        }


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
   }
}