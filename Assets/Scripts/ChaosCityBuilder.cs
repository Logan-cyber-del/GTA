using UnityEngine;

namespace ChaosCar
{
    [RequireComponent(typeof(Rigidbody))]
    public class ChaosCityBuilder : MonoBehaviour
    {
        [Header("Arena")]
        public float groundSize = 120f;
        public int policeCount = 4;

        [Header("Prefabs creados en escena")]
        public Material roadMaterial;
        public Material buildingMaterial;

        private void Awake()
        {
            CreateGround();
            CreateCity();
            CreateRamps();
            CreateSpikeStrips();
        }

        private void CreateGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(groundSize, 1f, groundSize);
            ground.transform.position = new Vector3(0f, -0.5f, 0f);

            for (int x = -40; x <= 40; x += 20)
            {
                GameObject roadX = GameObject.CreatePrimitive(PrimitiveType.Cube);
                roadX.name = "Avenida";
                roadX.transform.localScale = new Vector3(groundSize, 0.03f, 8f);
                roadX.transform.position = new Vector3(0f, 0.02f, x);
            }

            for (int z = -40; z <= 40; z += 20)
            {
                GameObject roadZ = GameObject.CreatePrimitive(PrimitiveType.Cube);
                roadZ.name = "Calle";
                roadZ.transform.localScale = new Vector3(8f, 0.03f, groundSize);
                roadZ.transform.position = new Vector3(z, 0.02f, 0f);
            }
        }

        private void CreateCity()
        {
            for (int x = -40; x <= 40; x += 20)
            {
                for (int z = -40; z <= 40; z += 20)
                {
                    Vector3[] corners =
                    {
                        new Vector3(x - 6f, 0f, z - 6f),
                        new Vector3(x + 6f, 0f, z - 6f),
                        new Vector3(x - 6f, 0f, z + 6f),
                        new Vector3(x + 6f, 0f, z + 6f)
                    };

                    foreach (Vector3 corner in corners)
                    {
                        float height = Random.Range(6f, 16f);
                        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        building.name = "Edificio";
                        building.transform.localScale = new Vector3(7f, height, 7f);
                        building.transform.position = corner + Vector3.up * height * 0.5f;
                    }
                }
            }
        }

        private void CreateRamps()
        {
            Vector3[] positions =
            {
                new Vector3(-18f, 0f, 28f),
                new Vector3(22f, 0f, -30f),
                new Vector3(34f, 0f, 10f)
            };

            foreach (Vector3 position in positions)
            {
                GameObject ramp = GameObject.CreatePrimitive(PrimitiveType.Cube);
                ramp.name = "Rampa caótica";
                ramp.transform.localScale = new Vector3(10f, 0.5f, 6f);
                ramp.transform.position = position + Vector3.up * 1.5f;
                ramp.transform.rotation = Quaternion.Euler(-25f, 0f, 0f);
            }
        }

        private void CreateSpikeStrips()
        {
            Vector3[] positions =
            {
                new Vector3(0f, 0f, 28f),
                new Vector3(-28f, 0f, 0f),
                new Vector3(28f, 0f, 20f)
            };

            foreach (Vector3 position in positions)
            {
                GameObject strip = GameObject.CreatePrimitive(PrimitiveType.Cube);
                strip.name = "Pinchos";
                strip.transform.localScale = new Vector3(5f, 0.12f, 1f);
                strip.transform.position = position + Vector3.up * 0.15f;
                strip.AddComponent<SpikeStrip>();
                strip.GetComponent<Collider>().isTrigger = true;
            }
        }
    }
}
