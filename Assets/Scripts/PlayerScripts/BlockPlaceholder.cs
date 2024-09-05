using System;
using BlockSystemScripts.BlockScripts;
using UnityEngine;

namespace PlayerScripts
{
    public class BlockPlaceholder : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;

        public void ToggleActive(BlockScript blockScript)
        {
            if (blockScript != null)
            {
                var materialFromBlock = blockScript.GetComponent<Renderer>().material;
                renderer.material = materialFromBlock;
                
                gameObject.SetActive(true);
            }

            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}