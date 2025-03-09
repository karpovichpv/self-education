import * as THREE from 'three';
import { color } from 'three/tsl';
import gsap from 'gsap';
import { Vector3 } from 'three/webgpu';

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
	width: 800,
	height: 600
}
// Camera

const camera = new THREE.PerspectiveCamera(75,sizes.width/sizes.height,1,5);
camera.position.z = 3
scene.add(camera);

// Renderer
const canvas = document.querySelector('.webgl');
const renderer = new THREE.WebGLRenderer({
	canvas: canvas
})
renderer.setSize(sizes.width, sizes.height);

const clock =new THREE.Clock();

// Animation
const tick = () =>{

	const elapsedTime = clock.getElapsedTime();

	// Update objects
	camera.position.x = Math.sin(cursor.x * Math.PI * 2) * 3
	camera.position.z = Math.sin(cursor.x * Math.PI * 2) * 3
	camera.position.y = cursor.y*5
	camera.lookAt(mesh.position)

	// Render
    renderer.render(scene, camera)

	window.requestAnimationFrame(tick)
}

tick()