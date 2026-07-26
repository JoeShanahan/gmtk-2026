#ifndef DISPLACEMENT_INCLUDED
#define DISPLACEMENT_INCLUDED

void displace_vertex_along_normals_float(
    float3 position,
    float3 normal,
    float displacement,
    float object_scale,
    out float3 out_position)
{
    out_position = position + normal * (displacement * object_scale);
}

void wobble_vertex_float(
    float3 position,
    float3 normal,
    float time,
    float wobble_amount,
    float wobble_speed,
    float wobble_frequency,
    float object_scale,
    out float3 out_position)
{
    out_position = position;

    if (wobble_amount <= 0)
        return;

    float wobble = sin(
        time * wobble_speed +
        length(position) * wobble_frequency
    );

    // Make sure it only goes outwards
    wobble = (wobble + 1) * 0.5;
    
    out_position += normal * wobble * wobble_amount * object_scale;
}

#endif