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
    public class FirstSteps : RenderCanvas
    {
        private TransformComponent _cubeTransform1;
        private TransformComponent _cubeTransform2;
        private TransformComponent _cubeTransform3;
        private TransformComponent _cubeTransform4;
        private TransformComponent _cubeTransform5;
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _camAngle = 0;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intentsity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0f, 0.139f, 0.139f, 1);

            // Create a scene with a cube
            // The three components: one XForm, one Material and the Mesh
            _cubeTransform1 = new TransformComponent { Scale = new float3(1, 1, 1), Translation = new float3(0, 0, 0) };
            var cubeMaterial1 = new MaterialComponent
            {

                Diffuse = new MatChannelContainer { Color = new float3(0.2f, 0.2f, 0.2f) },
                Specular = new SpecularChannelContainer { Color = float3.One, Shininess = 4 }

            };
            var cubeMesh1 = SimpleMeshes.CreateCuboid(new float3(5, 5, 5));

            _cubeTransform2 = new TransformComponent { Scale = new float3(1, 1, 1), Translation = new float3(0, 20, 0) };
            var cubeMaterial2 = new MaterialComponent
            {

                Diffuse = new MatChannelContainer { Color = new float3(1f, 0.2f, 0.2f) },
                Specular = new SpecularChannelContainer { Color = float3.One, Shininess = 4 }

            };
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(5, 20, 2));


            _cubeTransform3 = new TransformComponent { Scale = new float3(1, 1, 1), Translation = new float3(0, -20, 0) };
            var cubeMaterial3 = new MaterialComponent
            {

                Diffuse = new MatChannelContainer { Color = new float3(0.2f, 0.2f, 1f) },
                Specular = new SpecularChannelContainer { Color = float3.One, Shininess = 4 }

            };
            var cubeMesh3 = SimpleMeshes.CreateCuboid(new float3(5, 20, 2));

            _cubeTransform4 = new TransformComponent { Scale = new float3(1, 1, 1), Translation = new float3(-20, 0, 0) };
            var cubeMaterial4 = new MaterialComponent
            {

                Diffuse = new MatChannelContainer { Color = new float3(1f, 1f, 1f) },
                Specular = new SpecularChannelContainer { Color = float3.One, Shininess = 4 }

            };
            var cubeMesh4 = SimpleMeshes.CreateCuboid(new float3(10, 5, 5));

            _cubeTransform5 = new TransformComponent { Scale = new float3(1, 1, 1), Translation = new float3(20, 0, 0) };
            var cubeMaterial5 = new MaterialComponent
            {

                Diffuse = new MatChannelContainer { Color = new float3(0f, 0f, 0f) },
                Specular = new SpecularChannelContainer { Color = float3.One, Shininess = 4 }

            };
            var cubeMesh5 = SimpleMeshes.CreateCuboid(new float3(10, 5, 5));





            // Assemble the cube node containing the three components
            var cubeNode1 = new SceneNodeContainer();
            cubeNode1.Components = new List<SceneComponentContainer>();
            cubeNode1.Components.Add(_cubeTransform1);
            cubeNode1.Components.Add(cubeMaterial1);
            cubeNode1.Components.Add(cubeMesh1);

            var cubeNode2 = new SceneNodeContainer();
            cubeNode2.Components = new List<SceneComponentContainer>();
            cubeNode2.Components.Add(_cubeTransform2);
            cubeNode2.Components.Add(cubeMaterial2);
            cubeNode2.Components.Add(cubeMesh2);

            var cubeNode3 = new SceneNodeContainer();
            cubeNode3.Components = new List<SceneComponentContainer>();
            cubeNode3.Components.Add(_cubeTransform3);
            cubeNode3.Components.Add(cubeMaterial3);
            cubeNode3.Components.Add(cubeMesh3);

            var cubeNode4 = new SceneNodeContainer();
            cubeNode4.Components = new List<SceneComponentContainer>();
            cubeNode4.Components.Add(_cubeTransform4);
            cubeNode4.Components.Add(cubeMaterial4);
            cubeNode4.Components.Add(cubeMesh4);

            var cubeNode5 = new SceneNodeContainer();
            cubeNode5.Components = new List<SceneComponentContainer>();
            cubeNode5.Components.Add(_cubeTransform5);
            cubeNode5.Components.Add(cubeMaterial5);
            cubeNode5.Components.Add(cubeMesh5);

            // Create the scene containing the cubes
            _scene = new SceneContainer();
            _scene.Children = new List<SceneNodeContainer>();
            _scene.Children.Add(cubeNode1);
            _scene.Children.Add(cubeNode2);
            _scene.Children.Add(cubeNode3);
            _scene.Children.Add(cubeNode4);
            _scene.Children.Add(cubeNode5);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);


        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            _camAngle = _camAngle + 0.05f;

            _cubeTransform1.Scale = new float3(1 * M.Sin(4 * TimeSinceStart) + 1.5f, 1 * M.Sin(4 * TimeSinceStart) + 1.5f, 1 * M.Sin(4 * TimeSinceStart) + 1.5f);
            _cubeTransform1.Translation = new float3(0, 40 * M.Sin((2 * TimeSinceStart) + M.Pi / 4), 0);
            _cubeTransform2.Translation = new float3(0, 20 + 10 * M.Sin(1 * TimeSinceStart), 0);
            _cubeTransform3.Translation = new float3(0, -20 - 10 * M.Sin(1 * TimeSinceStart), 0);
            _cubeTransform4.Translation = new float3(20, 20 * M.Sin(6 * TimeSinceStart), 0);
            _cubeTransform5.Translation = new float3(-20, 20 * M.Sin(6 * TimeSinceStart), 0);


            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, 0, 100) * float4x4.CreateRotationY(_camAngle);



            // Render the scene on the current render context
            _sceneRenderer.Render(RC);


            // Swap buffers: Show the contents of the backbuffer (containing the currently rerndered farame) on the front buffer.
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