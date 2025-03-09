import * as THREE from 'three';
import { color } from 'three/tsl';
import gsap from 'gsap';

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

const camera = new THREE.PerspectiveCamera(45,sizes.width/sizes.height,1,5);
camera.position.z = 2
camera.position.y = 2
camera.position.x = 2
console.log(camera.position.length())
scene.add(camera);

// Renderer
const canvas = document.querySelector('.webgl');
console.log(canvas)
const renderer = new THREE.WebGLRenderer({
	canvas: canvas
})
renderer.setSize(sizes.width, sizes.height);

const clock =new THREE.Clock();

// Animation
const tick = () =>{

	const elapsedTime = clock.getElapsedTime();

	// Update objects
	camera.position.x = Math.cos(elapsedTime) 
	camera.position.y = Math.sin(elapsedTime) 
	camera.lookAt(mesh.position)

	// Render
    renderer.render(scene, camera)

	window.requestAnimationFrame(tick)
}

tick()