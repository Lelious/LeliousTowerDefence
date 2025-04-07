void SampleAnimation_float(in UnityTexture2D animationTexture,
							in float vertexId, in float time, in float frameRate,
							out float3 position)
{
	float frameCount = animationTexture.texelSize.z;
	float duration = frameCount / frameRate;
	float normalizedTime = (time / duration) % 1;

	float positionY = (vertexId + 0.5) * animationTexture.texelSize.y;
	float2 positionUv = float2(normalizedTime, positionY);
	position = animationTexture.SampleLevel(animationTexture.samplerstate, positionUv, 0).xyz;
}