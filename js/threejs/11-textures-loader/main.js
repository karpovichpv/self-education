import './style.css'
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import gsap from 'gsap'
import * as dat from 'dat.gui'

const gui = new dat.GUI({closed: true, width: 150});

const parameters = {
	spin: () => {
		gsap.to(mesh.rotation, {duration: 1, y:10})
	},
	color: 0xff0000
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

const textureLoader = new THREE.TextureLoader(loadingManager)
const texture = textureLoader.load(
	'./public/textures/door/color.jpg',
	() => {
		console.log('load')
	},
	() => {
		console.log('progress')
	},
	() => {
		console.log('error')
	}
)

const geometry = new THREE.BoxGeometry(1,1,1);
const material = new THREE.MeshBasicMaterial({ map: texture});

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

// Debug
// gui.hide()
gui.add(mesh.position,'y').min(-3).max(3).step(0.01).name('elevation')
gui.add(mesh.position,'x').min(-3).max(3).step(0.01)
gui.add(mesh.position,'z').min(-3).max(3).step(0.01)

gui.add(mesh, 'visible')

gui.add(mesh.material, 'wireframe')

gui.addColor(parameters, 'color').onChange(()=>{
	material.color.set(parameters.color)
})

gui.add(parameters, 'spin')