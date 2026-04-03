using UnityEngine;
using System.Collections;

public class Sensor_Bandit : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;

    public bool m_CanCheck = true;
    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
        /* Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.right * m_Bandit.inputX, 0.1f);
         if (colliders.Length > 0 )
         {
             return true;
         }
         else
         {
             return false;
         }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {

            m_ColCount++;
        
    }

    void OnTriggerExit2D(Collider2D other)
    {

            m_ColCount--;
        
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

    public IEnumerator cooldown()
    {
        this.enabled = false;
        yield return new WaitForSeconds(0.2f);
        this.enabled = false;
    }
}
