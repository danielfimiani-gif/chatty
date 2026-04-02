# Programación de Videojuegos Multijugador con Unity

## Trabajo Práctico 1

La entrega deberá consistir en un link a un repositorio de GitHub en donde se encuentre alojado un proyecto de Unity. Si bien puede ser desarrollado en cualquier versión del editor que se prefiera, la entrega debe poder ser abierta desde Unity 6 (versión 6000.0.36f1) y seguir funcionando debidamente. Asimismo, debe evidenciarse que el proyecto fue elaborado usando versionado (debe tener varios commits, a contraposición de uno solo con la consigna completada).

## Ejercicio

El proyecto debe consistir en una aplicación de mensajería de texto que funcione mediante conexión a Internet.

Al iniciar la aplicación, se deberá configurar si se desea iniciar como cliente o como cliente-servidor, usando dirección IP y puerto como parámetros.

Una vez configurada la conexión, el programa muestra una interfaz que debe tener, como mínimo:

- Un ScrollRect que permita visualizar todo el historial de mensajes.
- Un campo de texto que permita redactar un nuevo mensaje a enviar.
- Un botón que permita enviar el mensaje escrito.

Los clientes deben poder recibir los mensajes que envían otros clientes, siendo el servidor el intermediario entre ellos. El servidor puede actuar, si así se lo desea, como un cliente extra que también pueda enviar mensajes (aunque no es requisito).

La conexión debe realizarse, inicialmente, mediante el protocolo TCP, y solamente se podrán usar las clases que provee System.Net para ello (TcpClient, TcpListener, etc).

---

## Criterios de evaluación

Se dispone de un sistema de tiers, donde cada uno dispone de requisitos a cumplir para ser alcanzado. Contar con uno de los requisitos de un tier otorgará un punto. Con la finalidad de pasar de un tier a otro, es necesario cumplir con todos los requisitos indicados en él. Es decir, no se considerarán requisitos de un determinado tier si los pertenecientes al anterior no están todos logrados.

### Tier B: 4 (cuatro)

- [ ] El chat entre clientes funciona adecuadamente usando una conexión de red.
- [ ] Se utiliza una conexión TCP recurriendo solamente a las clases propias de C#.
- [ ] La interfaz de usuario permite elegir dirección IP y puerto para conectarse a un servidor.
- [ ] La interfaz de mensajería cuenta con un scroll que muestre el historial de mensajes; un campo de texto para redactar mensajes; y un botón para enviar mensajes escritos.

### Tier A: 7 (siete)

- [ ] El proyecto está estructurado de tal forma que se permite elegir si la conexión será mediante TCP o UDP, pudiendo llegar al mismo resultado con ambos protocolos de conexión.
- [ ] La interfaz de conexión permite elegir un nombre de usuario, y los mensajes se distribuyen en bloques dentro del scroll, donde a cada bloque se le antepone el emisor de ese mensaje (permitiendo entender mejor que se trata de un diálogo entre distintas personas).
- [ ] Los mensajes se pueden enviar en respuesta a un mensaje anterior del historial, y la interfaz deja en claro que ese mensaje hace referencia al anterior.

### Tier S: 10 (diez)

- [ ] El programa adapta su interfaz dependiendo de si corre en plataforma desktop o mobile.
- [ ] El programa funciona correctamente para conectar a clientes que no estén dentro de una misma red local (distintas direcciones IP públicas).
- [ ] El servidor se puede compilar para plataforma desktop en modo "sin gráficos", actuando simplemente como servidor para clientes en plataforma mobile.
