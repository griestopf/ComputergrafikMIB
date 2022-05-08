using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
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
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private float _cubeAngle = 0;
        private Camera _camera;
        private Transform _cubeTransform;
        private Transform _cameraTransform;


        // Init is called on startup. 
        public override void Init()
        {
            // Create a scene tree with one camera and one cube:
            // scene---+
            //         |
            //         +---cameraNode-------_cameraTransform---_camera
            //         |
            //         +---cubeNode---------_cubeTransform-----cubeEffect---cubeMesh

            // THE CAMERA
            // Two components: one Transform and one Camera component.
            _camera =  new Camera(ProjectionMethod.Perspective, 5, 100, M.PiOver4) {BackgroundColor = (float4) ColorUint.Greenery};
            _cameraTransform = new Transform{Translation = new float3(0, 0, -50)};
            var cameraNode = new SceneNode();
            cameraNode.Components.Add(_cameraTransform);
            cameraNode.Components.Add(_camera);

            // THE CUBE
            // Three components: one Transform, one SurfaceEffect (blue material) and the Mesh
            _cubeTransform = new Transform {Translation = new float3(0, 0, 0)};

            var cubeEffect = MakeEffect.FromDiffuseSpecular((float4) ColorUint.Blue);

            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            // Assemble the cube node containing the three components
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(cubeEffect);
            cubeNode.Components.Add(cubeMesh);

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cameraNode);
            _scene.Children.Add(cubeNode);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Animate the camera angle
            _cubeAngle = _cubeAngle + 90.0f * M.Pi/180.0f * DeltaTime ;

            // Animate the cube
            _cubeTransform.Rotation = new float3(0, _cubeAngle, 0);
            _cubeTransform.Translation = new float3(0, 5 * M.Sin(3 * TimeSinceStart), 0);

            // Render the scene tree
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
            Present();
        }
    }
}