import './style.css'
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

// Sizes

const sizes = {
	width: window.innerWidth,
	height: window.innerHeight, 
}

// Сursor
const cursor = {
	x : 0,
	y : 0
}
window.addEventListener('mousemove', (event) => {
	cursor.x = event.clientX / sizes.width - 0.5
	cursor.y = -(event.clientY / sizes.height - 0.5)
})

// Scene
const scene = new THREE.Scene();

const geometry = new THREE.BufferGeometry();
const count = 20
const positionsArray = new Float32Array(count * 3 * 3 )

for (let i=0; i<count * 3 * 3; i++)
{
	positionsArray[i] = (Math.random() - 0.5)*4

}

const positionsAttributes = new THREE.BufferAttribute(positionsArray, 3)
geometry.setAttribute('positions', positionsAttributes)

const material = new THREE.MeshBasicMaterial(
	{
		 color:'red',
		 wireframe: false,
		}
);
const mesh = new THREE.Mesh(geometry, material);
scene.add(mesh);


window.addEventListener('resize', () => {
	// Update sizes
	sizes.width = window.innerWidth
	sizes.height = window.innerHeight

	// Update the camera
	camera.aspect = sizes.width/sizes.height
	camera.updateProjectionMatrix()

    renderer.setSize(sizes.width, sizes.height)
	renderer.setPixelRatio(Math.min(window.devicePixelRatio,2))
})

window.addEventListener('dblclick', () => {
	const fullScreenElement = document.fullscreenElement || document.webkitFullScreenElement

	if (!fullScreenElement){
		if (canvas.requestFullscreen)
		{
					canvas.requestFullscreen();
		}
		else if (canvas.webkitRequestFullscreen)
		{
					canvas.webkitRequestFullscreen();
		}
	}
	else{
		if (document.exitFullscreen)
		{
					document.exitFullscreen();
		}
		else if (document.webkitExitFullscreen)
		{
					document.webkitExitFullscreen();
		}
	}
})

// Camera

const camera = new THREE.PerspectiveCamera(75,sizes.width/sizes.height,1,5);
camera.position.z = 3
scene.add(camera);

// Renderer
const canvas = document.querySelector('.webgl');
const renderer = new THREE.WebGLRenderer({
	canvas: canvas
})

// Controls 
const controls = new OrbitControls(camera, canvas)
controls.enableDamping = true
//controls.target.y = 2
//controls.update()

// Animation
const tick = () =>{

	// Update controls
	controls.update()

	// Render
    renderer.render(scene, camera)

	window.requestAnimationFrame(tick)
}

tick()