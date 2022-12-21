using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

class Damping
{
    float X_Damping, Y_Damping, Z_Damping;

    public Damping(CinemachineTransposer transposer)
    {
        Save(transposer);
    }

    private void Save(CinemachineTransposer transposer)
    {
        X_Damping = transposer.m_XDamping;
        Y_Damping = transposer.m_YDamping;
        Z_Damping = transposer.m_ZDamping;
    }

    public void Restore(CinemachineTransposer transposer)
    {
        transposer.m_XDamping = X_Damping;
        transposer.m_YDamping = Y_Damping;
        transposer.m_ZDamping = Z_Damping;
    }
}
