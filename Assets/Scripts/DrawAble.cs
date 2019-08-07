using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
public class DrawAble : MonoBehaviour
{


    public static Color Pen_Color = Color.red;     
    public static int Pen_Width = 1;
    public LayerMask Drawing_Layers;
    public static DrawAble drawable;
    Sprite drawable_sprite;
    public Texture2D drawable_texture;
    Color[] clean_colors_array;
    public bool Reset_Canvas_On_Play = true;
    public Color Reset_Color = new Color(0, 0, 0, 0); 
    Vector2 previous_drag_position;

    public Image FadeInOutImage;

    public string image_path;


    Color32[] cur_colors;

    public Stack<Color32[]> DrawStack = new Stack<Color32[]>();



    public void BackBt()
    {
        DrawStack.Pop();
        drawable_texture.SetPixels32(DrawStack.Peek());
        drawable_texture.Apply();
    }



    public void PenBrush(Vector2 world_point)
    {
        Vector2 pixel_pos = WorldToPixelCoordinates(world_point);

        cur_colors = drawable_texture.GetPixels32();



        if (previous_drag_position == Vector2.zero) //맨처음 클릭했을떄
        {

            MarkPixelsToColour(pixel_pos, Pen_Width, Pen_Color);
        }
        else // 드래그중일때
        {
            ColourBetween(previous_drag_position, pixel_pos, Pen_Width, Pen_Color);
            //ColourBetween(previous_drag_position, pixel_pos + new Vector2(40, 40), Pen_Width, Pen_Color);
            // ColourBetween(previous_drag_position, pixel_pos - new Vector2(40,0), Pen_Width, Pen_Color);

        }
        ApplyMarkedPixelChanges(); //적용

        previous_drag_position = pixel_pos;
    }
   
    public void Save_my()
    {
        Debug.Log("SAVE  " + this.name);

        if (Application.platform == RuntimePlatform.Android)
        {
            byte[] bytes = drawable_texture.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/Saved_Drawing_Charoctor/" + this.name + ".png", bytes);
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            byte[] bytes = drawable_texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Saved_Drawing_Charoctor/" + this.name + ".png", bytes);
        }
        else
        {
            byte[] bytes = drawable_texture.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/Resources/Saved_Drawing_Charoctor/" + this.name + ".png", bytes);
            File.WriteAllBytes(Application.dataPath + "/Saved_Drawing_Charoctor/" + this.name + ".png", bytes);

        }
        FadeInOutImage.gameObject.SetActive(true);
    }

   

    public void Reset_my()
    {
        Debug.Log("SAVE  " + this.name);
        ResetCanvas();
        Save_my();
    }

    void Update()
    {
        bool mouse_held_down = Input.GetMouseButton(0);

        Vector2 mouse_world_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouse_world_position, Drawing_Layers.value); //클릭했을 때 콜라이더 있는지 체크
            if (hit != null && hit.transform != null && hit.name == this.name)
            {
                DrawingSettings.nowDraw.Push(hit.gameObject);
                Debug.Log(DrawingSettings.nowDraw.Count);
            }
        }

        if (mouse_held_down)
        {

            Collider2D hit = Physics2D.OverlapPoint(mouse_world_position, Drawing_Layers.value); //클릭했을 때 콜라이더 있는지 체크

            if (hit != null && hit.transform != null && hit.name == this.name)
                PenBrush(mouse_world_position); //그리기
            else // 캔버스 벗어 났을 때
                previous_drag_position = Vector2.zero;
        }
        else if (Input.GetMouseButtonUp(0)) //마우스 땟을때
        {
            Collider2D hit = Physics2D.OverlapPoint(mouse_world_position, Drawing_Layers.value);
            if (hit != null && hit.transform != null && hit.name == this.name)
            {
                DrawStack.Push(drawable_texture.GetPixels32());
                previous_drag_position = Vector2.zero;
            }
        }
    }



    public void ColourBetween(Vector2 start_point, Vector2 end_point, int width, Color color)
    {
        float distance = Vector2.Distance(start_point, end_point);
        Vector2 direction = (start_point - end_point).normalized;

        Vector2 cur_position = start_point;
        float lerp_steps = 1 / distance;

        for (float lerp = 0; lerp <= 1; lerp += lerp_steps)
        {
            cur_position = Vector2.Lerp(start_point, end_point, lerp);
            MarkPixelsToColour(cur_position, width, color);
        }
    }





    public void MarkPixelsToColour(Vector2 center_pixel, int pen_thickness, Color color_of_pen)
    {
        int center_x = (int)center_pixel.x;
        int center_y = (int)center_pixel.y;

        for (int x = center_x - pen_thickness; x <= center_x + pen_thickness; x++)
        {
            if (x >= (int)drawable_sprite.rect.width || x < 0)
                continue;

            for (int y = center_y - pen_thickness; y <= center_y + pen_thickness; y++)
            {
                MarkPixelToChange(x, y, color_of_pen);
            }
        }
    }
    public void MarkPixelToChange(int x, int y, Color color)
    {
        int array_pos = y * (int)drawable_sprite.rect.width + x;

        if (array_pos > cur_colors.Length || array_pos < 0)
            return;

        cur_colors[array_pos] = color;
    }
    public void ApplyMarkedPixelChanges()
    {
        drawable_texture.SetPixels32(cur_colors);
        drawable_texture.Apply();
    }

    public Vector2 WorldToPixelCoordinates(Vector2 world_position)
    {
        Vector3 local_pos = transform.InverseTransformPoint(world_position);

        float pixelWidth = drawable_sprite.rect.width;
        float pixelHeight = drawable_sprite.rect.height;
        float unitsToPixels = pixelWidth / drawable_sprite.bounds.size.x * transform.localScale.x;

        float centered_x = local_pos.x * unitsToPixels + pixelWidth / 2;
        float centered_y = local_pos.y * unitsToPixels + pixelHeight / 2;

        Vector2 pixel_pos = new Vector2(Mathf.RoundToInt(centered_x), Mathf.RoundToInt(centered_y));

        return pixel_pos;
    }


    public void ResetCanvas()
    {
        drawable_texture.SetPixels(clean_colors_array);
        drawable_texture.Apply();
    }



    void Start()
    {
        //if (Application.platform == RuntimePlatform.WindowsPlayer)
        //{
        //    string temp_url = "file://"; /// www 를 쓰기 위해선 uri 경로 앞에 file:// 붙어야함

        //    string url = Application.dataPath + "/Resources/Saved_Drawing_Charoctor/" + this.name + ".png"; // 사진이 있는 경로
        //    www = new WWW(temp_url + url);
        //    Texture2D t = www.texture;
        //    Rect rect = new Rect(0, 0, t.width, t.height);
        //    GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, rect, new Vector2(0.5f, 0.5f));
        //}
        //else
        //GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Saved_Drawing_Charoctor/" + this.name);
        //WWW www;

        //string temp_url = "file://"; /// www 를 쓰기 위해선 uri 경로 앞에 file:// 붙어야함
        //string url = Application.dataPath + "/Saved_Drawing_Charoctor/" + this.name + ".png"; // 사진이 있는 경로
        //www = new WWW(temp_url + url);
        //Texture2D t = www.texture;
        //Rect rect = new Rect(0, 0, t.width, t.height);
        //GetComponent<SpriteRenderer>().sprite = Sprite.Create(t, rect, new Vector2(0.5f, 0.5f));

        drawable = this;

        drawable_sprite = this.GetComponent<SpriteRenderer>().sprite;
        drawable_texture = drawable_sprite.texture;

        DrawStack.Push(drawable_texture.GetPixels32());
    

        clean_colors_array = new Color[(int)drawable_sprite.rect.width * (int)drawable_sprite.rect.height]; //그리는 부분의 픽셀의 겟수가 배열 크기
        for (int x = 0; x < clean_colors_array.Length; x++)
        {
            clean_colors_array[x] = Reset_Color;
        }

        if (Reset_Canvas_On_Play) //캔버스 리셋
            ResetCanvas();
    }

}
