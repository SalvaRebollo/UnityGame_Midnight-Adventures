using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject portal2D;
    public GameObject player;
    private bool portalDeshabilitado;

    public AudioClip portalSfx;
    private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetComponent<PlayerControl>().canUsePortals)
        {
            audioSource.PlayOneShot(portalSfx);
            collision.GetComponent<PlayerControl>().canUsePortals = false;
            StartCoroutine(transport());
            Invoke("HabilitarPortal", 0.5f);
        }
    }

    IEnumerator transport()
    {
        yield return new WaitForSeconds(0.05f);
        player.transform.position = new Vector2(portal2D.transform.position.x, portal2D.transform.position.y);
    }

    private void HabilitarPortal()
    {
        player.GetComponent<PlayerControl>().canUsePortals = true;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
