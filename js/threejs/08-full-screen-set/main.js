import './style.css'
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

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

// Red cube
const geometry = new THREE.BoxGeometry(1,1,1);
const material = new THREE.MeshBasicMaterial({ color:'red'});
const mesh = new THREE.Mesh(geometry, material);
scene.add(mesh);

// Sizes

const sizes = {
	width: window.innerWidth,
	height: window.innerHeight, 
}

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