import './style.css'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

/**
 * Textures
 */

const textureLoadingManager = new THREE.LoadingManager();

textureLoadingManager.onStart = ()=>{
	console.log('start')
}
textureLoadingManager.onLoad = ()=>{
	console.log('load')
}
textureLoadingManager.onProgress = ()=>{
	console.log('progress')
}
textureLoadingManager.onError = (e)=>{
	console.log('can not find'+e)
}

const textureLoader = new THREE.TextureLoader(textureLoadingManager)
const doorColorTexture = textureLoader.load('./public/textures/door/color.jpg')
const doorAlphaTexture = textureLoader.load('./public/textures/door/alpha.jpg')
const doorAmbientOcclusionTexture = textureLoader.load('./public/textures/door/ambientOcclusion.jpg')
const doorHeightTexture = textureLoader.load('./public/textures/door/height.jpg')
const doorNormalTexture = textureLoader.load('./public/textures/door/normal.jpg')
const doorMetallnessTexture = textureLoader.load('./public/textures/door/metalness.jpg')
const doorRoughnessTexture = textureLoader.load('./public/textures/door/roughness.jpg')
const matcapTexture = textureLoader.load('./public/textures/matcaps/1.png')
const gradientTexture = textureLoader.load('./public/textures/gradients/5.jpg')

/**
 * Base
 */
// Canvas
const canvas = document.querySelector('canvas.webgl')

// Scene
const scene = new THREE.Scene()

/**
 * Lights
 */

const light = new THREE.AmbientLight( 0xffffff, 0.75 ); // soft white light
scene.add( light );

const pointLight = new THREE.PointLight( 0xffffff, 0.5 );
pointLight.position.set(3, 3, 3);
scene.add( pointLight );

/**
 * Objects
 */

//const material = new THREE.MeshBasicMaterial( )
//material.map = doorColorTexture
//material.alphaMap = doorAlphaTexture
//material.transparent = true
//material.side = THREE.BackSide

//const material = new THREE.MeshNormalMaterial();

//const material = new THREE.MeshMatcapMaterial();
//material.matcapTexture = doorAlphaTexture

//const material = new THREE.MeshDepthMaterial()

//const material = new THREE.MeshPhongMaterial({
//    color: "#ffffff",
//    map: gradientTexture
//})
//material.shininess = 300
//material.specular = new THREE.Color(0xff00000)

//const material = new THREE.MeshToonMaterial()
//material.gradientMap = gradientTexture

const material = new THREE.MeshStandardMaterial()
//material.metalness = 0.1
//material.roughness = 0.1
material.map = doorColorTexture
material.aoMap = doorAmbientOcclusionTexture
material.aoMapIntensity = 2 
material.displacementMap = doorHeightTexture
material.displacementScale = 0.05
material.metalnessMap = doorMetallnessTexture
material.roughnessMap = doorRoughnessTexture
material.normalMap = doorNormalTexture
material.normalScale.set(5, 5)
material.transparent = true
material.alphaMap = doorAlphaTexture

material.wireframe = false

const sphere = new THREE.Mesh(
	new THREE.SphereGeometry(0.5,128,128),
	material
)
sphere.position.x = - 1.5

sphere.geometry.setAttribute(
    'uv2',
    new THREE.BufferAttribute(sphere.geometry.attributes.uv.array, 2)
    )

const plane = new THREE.Mesh(
	new THREE.PlaneGeometry(1,1, 100, 100),
	material
)

plane.geometry.setAttribute(
    'uv2',
    new THREE.BufferAttribute(plane.geometry.attributes.uv.array, 2)
    )


const torus = new THREE.Mesh(
	new THREE.TorusGeometry(0.3, 0.2, 128, 128),
	material
)
torus.position.x = 1.5
torus.geometry.setAttribute(
    'uv2',
    new THREE.BufferAttribute(torus.geometry.attributes.uv.array, 2)
    )

scene.add(sphere, plane, torus)

/**
 * Sizes
 */
const sizes = {
    width: window.innerWidth,
    height: window.innerHeight
}

window.addEventListener('resize', () =>
{
    // Update sizes
    sizes.width = window.innerWidth
    sizes.height = window.innerHeight

    // Update camera
    camera.aspect = sizes.width / sizes.height
    camera.updateProjectionMatrix()

    // Update renderer
    renderer.setSize(sizes.width, sizes.height)
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))
})

/**
 * Camera
 */
// Base camera
const camera = new THREE.PerspectiveCamera(75, sizes.width / sizes.height, 0.1, 100)
camera.position.x = 1
camera.position.y = 1
camera.position.z = 2
scene.add(camera)

// Controls
const controls = new OrbitControls(camera, canvas)
controls.enableDamping = true

/**
 * Renderer
 */
const renderer = new THREE.WebGLRenderer({
    canvas: canvas
})
renderer.setSize(sizes.width, sizes.height)
renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2))

/**
 * Animate
 */
const clock = new THREE.Clock()

const tick = () =>
{
    const elapsedTime = clock.getElapsedTime()

	// Update objects
	sphere.rotation.y = 0.1 * elapsedTime
	plane.rotation.y = 0.1 * elapsedTime
	torus.rotation.y = 0.1 * elapsedTime

	sphere.rotation.z = 0.2 * elapsedTime
	plane.rotation.z = 0.2 * elapsedTime
	torus.rotation.z = 0.2 * elapsedTime

    // Update controls
    controls.update()

    // Render
    renderer.render(scene, camera)

    // Call tick again on the next frame
    window.requestAnimationFrame(tick)
}

tick()