# Prototipo de Realidad Virtual

### Datos de los alumnos
- Igor Dragone, alu0101469652@ull.edu.es
- Álvaro Fontenla León, alu0101437989@ull.edu.es
- Sergio Pérez Lozano, alu0101473260@ull.edu.es
- Juan Rodríguez Suárez, alu0101477596@ull.edu.es

## Cuestiones importantes para el uso
El prototipo ha sido desarrollado para Android. En primer lugar, vamos a explorar las configuraciones necesarias para permitir la construcción de la APK. El proyecto se ha configurado para permitir el uso del [Google Cardboard SDK Plugin](https://developers.google.com/cardboard/develop/unity/quickstart), por lo que se deben añadir las siguientes configuraciones:
- En `File > Build Settings`, seleccionamos `Android`
- En `Project Settings > Player > Resolution and Presentation`:
  - `Default Orientation -> Landscape Left`
  - Desactivamos `Optimized Frame Pacing`
- En `Project Settings > Player > Other Settings`:
  - Elegimos `OpenGLES3` en `GraphicsAPI` (Importante quitar `Vulkan`)
  - `Minimum API Level >= Android 8.0 'Oreo' (API level 26)`
  - `Target API Level >= API level 34`
  - `Scripting Backend -> IL2CPP`
  - `Target Architectures -> ARM64`
  - `Internet Access -> Require`
  - `Active Input Handling -> Input System Package (New)`
  - `Application Entry Poin -> Activity`
- En `Project Settings > XR Plug-in Management`, elegimos `Cardboard XR Plugin` en Plug-in Providers

Pasamos ahora a las formas de probar el prototipo. Hemos permitido 3 posibilidades:
- Con un móvil Android y el Google Cardboard VR Headset (Recomendado). Introducimos el móvil dentro del Cardboard y nos colocamos el aparato. De esta forma nos veremos sumergidos en el juego. 
- Únicamente con un móvil Android. En el caso de que nos falte el Cardboard, podemos probar la aplicación solo con el móvil, aunque la experiencia será más limitada.
- Con el ordenador. Por razones de depuración se ha añadido la posibilidad de ejecutar la aplicación directamente desde Unity. Para ello necesitaremos 2 cosas: el script `CardboardSimulator.cs` y el ajuste ` Active Input Handling -> Both` (Project Settings > Player > Other Settings)

## Hitos de programación logrados 
Relacionados con los contenidos que se han impartido
Selección con mirada, elementos de UI, escenario, eventos, sonidos

## Aspectos a destacar en la aplicación. 
Selección con mirada, menú inicial, juego de preguntas
## Integración de sensores
Especificar si se han incluido sensores de los que se han trabajado en interfaces multimodales.
## Gif animado de ejecución
## Acta de los acuerdos del grupo respecto al trabajo en equipo
Se ha ido desarrollando el prototipo de forma incremental. La metodología seguida es la siguiente:
1. En grupo, se elige la idea del prototipo y el orden en el que trabajará cada integrante del grupo
2. El primer integrante desarrolla el esqueleto de la aplicación. Una vez acabado, se pone en contacto con el segundo y juntos discuten que puede añadir este último. Una vez llegados a una conclusión, la antorcha pasa del primer integrante al segundo, quien es ahora el responsable de añadir nuevas funcionalidades a la aplicación. Se repite este proceso hasta llegar al último integrante del grupo. Una vez finalizada esta sesión, todo el equipo revisa el trabajo realizado y, una vez satisfecho, da el prototipo como terminado.

Entre las diferentes funcionalidades añadidas de forma incremental, destacan (en orden):
- Creación del esqueleto inicial: esqueleto, público, tablet, pantalla
- Creación de la primera mecánica de juego: 15 preguntas de 4 opciones, se debe acertar la correcta para pasar a la siguiente
- Adición de sonidos en el juego: música de fondo, etc.
- Aparición de 3 comodines
- Creación de un menú inicial
- Extensión de la base de datos de preguntas a través de uns API
- Nueva mecánica de victoria
