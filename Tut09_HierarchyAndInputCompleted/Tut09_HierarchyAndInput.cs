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
        private Transform _bodyTransform;
        private Transform _upperArmTransform;

        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new Transform
            {
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new Transform
            {
                Translation = new float3(0, 6, 0),
                Rotation = new float3(0, 0.2f, 0)
            };

            _upperArmTransform = new Transform
            {
                Translation = new float3(2, 4, 0),
                Rotation = new float3(0.76f, 0, 0)
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
                        Name = "Base (grey)",
                        Components = 
                        {
                            _baseTransform,
                            MakeEffect.FromDiffuseSpecular((float4) ColorUint.LightGrey),
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        },
                        Children =
                        {
                            new SceneNode
                            {
                                Name = "Body (red)",
                                Components = 
                                {
                                    _bodyTransform,
                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.IndianRed),
                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                },
                                Children =
                                {
                                    new SceneNode
                                    {
                                        Name = "Upper Arm (green)",
                                        Components = 
                                        {
                                            _upperArmTransform,
                                        },
                                        Children = 
                                        {
                                            new SceneNode
                                            {
                                                Components =
                                                {
                                                    new Transform { Translation = new float3(0, 4, 0)},
                                                    MakeEffect.FromDiffuseSpecular((float4) ColorUint.ForestGreen),
                                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                }
                                            },
                                        }
                                    }
                                }
                            }
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
            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 0.1f * Keyboard.LeftRightAxis;
            _bodyTransform.Rotation = new float3(0, bodyRot, 0);

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
   }
}