import bpy
import os

def kmi_props_setattr(kmi_props, attr, value):
    try:
        setattr(kmi_props, attr, value)
    except AttributeError:
        print("Warning: property '%s' not found in keymap item '%s'" %
              (attr, kmi_props.__class__.__name__))
    except Exception as e:
        print("Warning: %r" % e)

wm = bpy.context.window_manager
kc = wm.keyconfigs.new(os.path.splitext(os.path.basename(__file__))[0])

# Map 3D View
km = kc.keymaps.new('3D View', space_type='VIEW_3D', region_type='WINDOW', modal=False)

kmi = km.keymap_items.new('view3d.screencast_keys', 'C', 'PRESS', shift=True, alt=True)
kmi = km.keymap_items.new('view3d.manipulator', 'LEFTMOUSE', 'PRESS', any=True)
kmi_props_setattr(kmi.properties, 'release_confirm', True)
kmi = km.keymap_items.new('view3d.cursor3d', 'ACTIONMOUSE', 'PRESS')
kmi.active = False
kmi = km.keymap_items.new('view3d.rotate', 'MIDDLEMOUSE', 'PRESS', alt=True)
kmi = km.keymap_items.new('view3d.move', 'MIDDLEMOUSE', 'PRESS')
kmi = km.keymap_items.new('view3d.zoom', 'MIDDLEMOUSE', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('view3d.dolly', 'MIDDLEMOUSE', 'PRESS', shift=True, ctrl=True)
kmi = km.keymap_items.new('view3d.view_selected', 'NUMPAD_PERIOD', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'use_all_regions', True)
kmi = km.keymap_items.new('view3d.view_selected', 'NUMPAD_PERIOD', 'PRESS')
kmi_props_setattr(kmi.properties, 'use_all_regions', False)
kmi = km.keymap_items.new('view3d.view_lock_to_active', 'NUMPAD_PERIOD', 'PRESS', shift=True)
kmi = km.keymap_items.new('view3d.view_lock_clear', 'NUMPAD_PERIOD', 'PRESS', alt=True)
kmi = km.keymap_items.new('view3d.navigate', 'F', 'PRESS', shift=True)
kmi = km.keymap_items.new('view3d.smoothview', 'TIMER1', 'ANY', any=True)
kmi = km.keymap_items.new('view3d.rotate', 'TRACKPADPAN', 'ANY')
kmi = km.keymap_items.new('view3d.rotate', 'MOUSEROTATE', 'ANY')
kmi = km.keymap_items.new('view3d.move', 'TRACKPADPAN', 'ANY', shift=True)
kmi = km.keymap_items.new('view3d.zoom', 'TRACKPADZOOM', 'ANY')
kmi = km.keymap_items.new('view3d.zoom', 'TRACKPADPAN', 'ANY', ctrl=True)
kmi = km.keymap_items.new('view3d.zoom', 'NUMPAD_PLUS', 'PRESS')
kmi_props_setattr(kmi.properties, 'delta', 1)
kmi = km.keymap_items.new('view3d.zoom', 'NUMPAD_MINUS', 'PRESS')
kmi_props_setattr(kmi.properties, 'delta', -1)
kmi = km.keymap_items.new('view3d.zoom', 'EQUAL', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'delta', 1)
kmi = km.keymap_items.new('view3d.zoom', 'MINUS', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'delta', -1)
kmi = km.keymap_items.new('view3d.zoom', 'WHEELINMOUSE', 'PRESS')
kmi_props_setattr(kmi.properties, 'delta', 1)
kmi = km.keymap_items.new('view3d.zoom', 'WHEELOUTMOUSE', 'PRESS')
kmi_props_setattr(kmi.properties, 'delta', -1)
kmi = km.keymap_items.new('view3d.dolly', 'NUMPAD_PLUS', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'delta', 1)
kmi = km.keymap_items.new('view3d.dolly', 'NUMPAD_MINUS', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'delta', -1)
kmi = km.keymap_items.new('view3d.dolly', 'EQUAL', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'delta', 1)
kmi = km.keymap_items.new('view3d.dolly', 'MINUS', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'delta', -1)
kmi = km.keymap_items.new('view3d.zoom_camera_1_to_1', 'NUMPAD_ENTER', 'PRESS', shift=True)
kmi = km.keymap_items.new('view3d.view_center_camera', 'HOME', 'PRESS')
kmi = km.keymap_items.new('view3d.view_center_lock', 'HOME', 'PRESS')
kmi = km.keymap_items.new('view3d.view_center_cursor', 'HOME', 'PRESS', alt=True)
kmi = km.keymap_items.new('view3d.view_center_pick', 'F', 'PRESS', alt=True)
kmi = km.keymap_items.new('view3d.view_all', 'HOME', 'PRESS')
kmi_props_setattr(kmi.properties, 'center', False)
kmi = km.keymap_items.new('view3d.view_all', 'HOME', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'use_all_regions', True)
kmi_props_setattr(kmi.properties, 'center', False)
kmi = km.keymap_items.new('view3d.view_all', 'C', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'center', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_0', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'CAMERA')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_1', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'FRONT')
kmi = km.keymap_items.new('view3d.view_orbit', 'NUMPAD_2', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'ORBITDOWN')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_3', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'RIGHT')
kmi = km.keymap_items.new('view3d.view_orbit', 'NUMPAD_4', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'ORBITLEFT')
kmi = km.keymap_items.new('view3d.view_persportho', 'NUMPAD_5', 'PRESS')
kmi = km.keymap_items.new('view3d.view_orbit', 'NUMPAD_6', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'ORBITRIGHT')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_7', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'TOP')
kmi = km.keymap_items.new('view3d.view_orbit', 'NUMPAD_8', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'ORBITUP')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_1', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'BACK')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_3', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'LEFT')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_7', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'BOTTOM')
kmi = km.keymap_items.new('view3d.view_pan', 'NUMPAD_2', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANDOWN')
kmi = km.keymap_items.new('view3d.view_pan', 'NUMPAD_4', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANLEFT')
kmi = km.keymap_items.new('view3d.view_pan', 'NUMPAD_6', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANRIGHT')
kmi = km.keymap_items.new('view3d.view_pan', 'NUMPAD_8', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANUP')
kmi = km.keymap_items.new('view3d.view_roll', 'NUMPAD_4', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'angle', -0.2617993950843811)
kmi = km.keymap_items.new('view3d.view_roll', 'NUMPAD_6', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'angle', 0.2617993950843811)
kmi = km.keymap_items.new('view3d.view_pan', 'WHEELUPMOUSE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANRIGHT')
kmi = km.keymap_items.new('view3d.view_pan', 'WHEELDOWNMOUSE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'PANLEFT')
kmi = km.keymap_items.new('view3d.view_pan', 'WHEELUPMOUSE', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'PANUP')
kmi = km.keymap_items.new('view3d.view_pan', 'WHEELDOWNMOUSE', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'PANLEFT')
kmi = km.keymap_items.new('view3d.view_orbit', 'WHEELUPMOUSE', 'PRESS', ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'type', 'ORBITLEFT')
kmi = km.keymap_items.new('view3d.view_orbit', 'WHEELDOWNMOUSE', 'PRESS', ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'type', 'ORBITRIGHT')
kmi = km.keymap_items.new('view3d.view_orbit', 'WHEELUPMOUSE', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'type', 'ORBITUP')
kmi = km.keymap_items.new('view3d.view_orbit', 'WHEELDOWNMOUSE', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'type', 'ORBITDOWN')
kmi = km.keymap_items.new('view3d.view_roll', 'WHEELUPMOUSE', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'angle', -0.2617993950843811)
kmi = km.keymap_items.new('view3d.view_roll', 'WHEELDOWNMOUSE', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'angle', 0.2617993950843811)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_1', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'FRONT')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_3', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'RIGHT')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_7', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'TOP')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_1', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'BACK')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_3', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'LEFT')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NUMPAD_7', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'type', 'BOTTOM')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.localview', 'NUMPAD_SLASH', 'PRESS')
kmi = km.keymap_items.new('view3d.ndof_orbit_zoom', 'NDOF_MOTION', 'ANY')
kmi = km.keymap_items.new('view3d.ndof_orbit', 'NDOF_MOTION', 'ANY', ctrl=True)
kmi = km.keymap_items.new('view3d.ndof_pan', 'NDOF_MOTION', 'ANY', shift=True)
kmi = km.keymap_items.new('view3d.ndof_all', 'NDOF_MOTION', 'ANY', shift=True, ctrl=True)
kmi = km.keymap_items.new('view3d.view_selected', 'NDOF_BUTTON_FIT', 'PRESS')
kmi_props_setattr(kmi.properties, 'use_all_regions', False)
kmi = km.keymap_items.new('view3d.view_roll', 'NDOF_BUTTON_ROLL_CCW', 'PRESS')
kmi_props_setattr(kmi.properties, 'angle', -1.5707963705062866)
kmi = km.keymap_items.new('view3d.view_roll', 'NDOF_BUTTON_ROLL_CW', 'PRESS')
kmi_props_setattr(kmi.properties, 'angle', 1.5707963705062866)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_FRONT', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'FRONT')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_BACK', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'BACK')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_LEFT', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'LEFT')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_RIGHT', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'RIGHT')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_TOP', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'TOP')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_BOTTOM', 'PRESS')
kmi_props_setattr(kmi.properties, 'type', 'BOTTOM')
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_FRONT', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'FRONT')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_RIGHT', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'RIGHT')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.viewnumpad', 'NDOF_BUTTON_TOP', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'type', 'TOP')
kmi_props_setattr(kmi.properties, 'align_active', True)
kmi = km.keymap_items.new('view3d.layers', 'ACCENT_GRAVE', 'PRESS')
kmi_props_setattr(kmi.properties, 'nr', 0)
kmi = km.keymap_items.new('wm.context_toggle_enum', 'Z', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.viewport_shade')
kmi_props_setattr(kmi.properties, 'value_1', 'SOLID')
kmi_props_setattr(kmi.properties, 'value_2', 'WIREFRAME')
kmi = km.keymap_items.new('wm.context_toggle_enum', 'Z', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.viewport_shade')
kmi_props_setattr(kmi.properties, 'value_1', 'SOLID')
kmi_props_setattr(kmi.properties, 'value_2', 'TEXTURED')
kmi = km.keymap_items.new('wm.context_toggle_enum', 'Z', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.viewport_shade')
kmi_props_setattr(kmi.properties, 'value_1', 'SOLID')
kmi_props_setattr(kmi.properties, 'value_2', 'RENDERED')
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS')
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi_props_setattr(kmi.properties, 'center', False)
kmi_props_setattr(kmi.properties, 'enumerate', False)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi_props_setattr(kmi.properties, 'center', False)
kmi_props_setattr(kmi.properties, 'enumerate', False)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi_props_setattr(kmi.properties, 'center', True)
kmi_props_setattr(kmi.properties, 'enumerate', False)
kmi_props_setattr(kmi.properties, 'object', True)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi_props_setattr(kmi.properties, 'center', False)
kmi_props_setattr(kmi.properties, 'enumerate', True)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'extend', True)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi_props_setattr(kmi.properties, 'center', True)
kmi_props_setattr(kmi.properties, 'enumerate', False)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi_props_setattr(kmi.properties, 'center', True)
kmi_props_setattr(kmi.properties, 'enumerate', True)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi_props_setattr(kmi.properties, 'center', False)
kmi_props_setattr(kmi.properties, 'enumerate', True)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select', 'SELECTMOUSE', 'PRESS', shift=True, ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi_props_setattr(kmi.properties, 'center', True)
kmi_props_setattr(kmi.properties, 'enumerate', True)
kmi_props_setattr(kmi.properties, 'object', False)
kmi = km.keymap_items.new('view3d.select_border', 'B', 'PRESS')
kmi = km.keymap_items.new('view3d.select_lasso', 'EVT_TWEAK_A', 'ANY', ctrl=True)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi = km.keymap_items.new('view3d.select_lasso', 'EVT_TWEAK_A', 'ANY', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'deselect', True)
kmi = km.keymap_items.new('view3d.select_circle', 'C', 'PRESS')
kmi = km.keymap_items.new('view3d.clip_border', 'B', 'PRESS', alt=True)
kmi = km.keymap_items.new('view3d.zoom_border', 'B', 'PRESS', shift=True)
kmi = km.keymap_items.new('view3d.render_border', 'B', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'camera_only', True)
kmi = km.keymap_items.new('view3d.render_border', 'B', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'camera_only', False)
kmi = km.keymap_items.new('view3d.clear_render_border', 'B', 'PRESS', ctrl=True, alt=True)
kmi = km.keymap_items.new('view3d.camera_to_view', 'NUMPAD_0', 'PRESS', ctrl=True, alt=True)
kmi = km.keymap_items.new('view3d.object_as_camera', 'NUMPAD_0', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('wm.call_menu', 'S', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_snap')
kmi = km.keymap_items.new('view3d.copybuffer', 'C', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('view3d.pastebuffer', 'V', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('wm.context_set_enum', 'COMMA', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.pivot_point')
kmi_props_setattr(kmi.properties, 'value', 'BOUNDING_BOX_CENTER')
kmi = km.keymap_items.new('wm.context_set_enum', 'COMMA', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.pivot_point')
kmi_props_setattr(kmi.properties, 'value', 'MEDIAN_POINT')
kmi = km.keymap_items.new('wm.context_toggle', 'COMMA', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.use_pivot_point_align')
kmi = km.keymap_items.new('wm.context_toggle', 'SPACE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.show_manipulator')
kmi = km.keymap_items.new('wm.context_set_enum', 'PERIOD', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.pivot_point')
kmi_props_setattr(kmi.properties, 'value', 'CURSOR')
kmi = km.keymap_items.new('wm.context_set_enum', 'PERIOD', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.pivot_point')
kmi_props_setattr(kmi.properties, 'value', 'INDIVIDUAL_ORIGINS')
kmi = km.keymap_items.new('wm.context_set_enum', 'PERIOD', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'data_path', 'space_data.pivot_point')
kmi_props_setattr(kmi.properties, 'value', 'ACTIVE_ELEMENT')
kmi = km.keymap_items.new('transform.translate', 'G', 'PRESS')
kmi = km.keymap_items.new('transform.translate', 'EVT_TWEAK_S', 'ANY')
kmi = km.keymap_items.new('transform.rotate', 'R', 'PRESS')
kmi = km.keymap_items.new('transform.resize', 'S', 'PRESS')
kmi = km.keymap_items.new('transform.bend', 'W', 'PRESS', shift=True)
kmi = km.keymap_items.new('transform.tosphere', 'S', 'PRESS', shift=True, alt=True)
kmi = km.keymap_items.new('transform.shear', 'S', 'PRESS', shift=True, ctrl=True, alt=True)
kmi = km.keymap_items.new('transform.select_orientation', 'SPACE', 'PRESS', alt=True)
kmi = km.keymap_items.new('transform.create_orientation', 'SPACE', 'PRESS', ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'use', True)
kmi = km.keymap_items.new('transform.mirror', 'M', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('wm.context_toggle', 'TAB', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.use_snap')
kmi = km.keymap_items.new('wm.context_menu_enum', 'TAB', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.snap_element')
kmi = km.keymap_items.new('transform.translate', 'T', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'texture_space', True)
kmi = km.keymap_items.new('transform.resize', 'T', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'texture_space', True)
kmi = km.keymap_items.new('transform.skin_resize', 'A', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('none', 'A', 'PRESS')
kmi = km.keymap_items.new('view3d.cursor3d_enhanced', 'ACTIONMOUSE', 'PRESS')
kmi = km.keymap_items.new('view3d.cursor3d_enhanced', 'F10', 'PRESS')

# Map Mesh
km = kc.keymaps.new('Mesh', space_type='EMPTY', region_type='WINDOW', modal=False)

kmi = km.keymap_items.new('wm.call_menu', 'C', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'MESH_MT_CopyFaceSettings')
kmi = km.keymap_items.new('mesh.loopcut_slide', 'R', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.inset', 'I', 'PRESS')
kmi = km.keymap_items.new('mesh.poke', 'P', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.bevel', 'B', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'vertex_only', False)
kmi = km.keymap_items.new('mesh.bevel', 'B', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'vertex_only', True)
kmi = km.keymap_items.new('mesh.loop_select', 'SELECTMOUSE', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi = km.keymap_items.new('mesh.loop_select', 'SELECTMOUSE', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi = km.keymap_items.new('mesh.edgering_select', 'SELECTMOUSE', 'PRESS', ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', False)
kmi = km.keymap_items.new('mesh.edgering_select', 'SELECTMOUSE', 'PRESS', shift=True, ctrl=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi_props_setattr(kmi.properties, 'toggle', True)
kmi = km.keymap_items.new('mesh.shortest_path_pick', 'SELECTMOUSE', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.select_all', 'A', 'PRESS')
kmi_props_setattr(kmi.properties, 'action', 'TOGGLE')
kmi = km.keymap_items.new('mesh.select_all', 'I', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'action', 'INVERT')
kmi = km.keymap_items.new('mesh.select_more', 'NUMPAD_PLUS', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.select_less', 'NUMPAD_MINUS', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.select_non_manifold', 'M', 'PRESS', shift=True, ctrl=True, alt=True)
kmi = km.keymap_items.new('mesh.select_linked', 'L', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.select_linked_pick', 'L', 'PRESS')
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi = km.keymap_items.new('mesh.select_linked_pick', 'L', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'deselect', True)
kmi = km.keymap_items.new('mesh.faces_select_linked_flat', 'F', 'PRESS', shift=True, ctrl=True, alt=True)
kmi = km.keymap_items.new('mesh.select_similar', 'G', 'PRESS', shift=True)
kmi = km.keymap_items.new('wm.call_menu', 'TAB', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_select_mode')
kmi = km.keymap_items.new('mesh.hide', 'H', 'PRESS')
kmi_props_setattr(kmi.properties, 'unselected', False)
kmi = km.keymap_items.new('mesh.hide', 'H', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'unselected', True)
kmi = km.keymap_items.new('mesh.reveal', 'H', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.normals_make_consistent', 'N', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'inside', False)
kmi = km.keymap_items.new('mesh.normals_make_consistent', 'N', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'inside', True)
kmi = km.keymap_items.new('view3d.edit_mesh_extrude_move_normal', 'E', 'PRESS')
kmi = km.keymap_items.new('wm.call_menu', 'E', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_extrude')
kmi = km.keymap_items.new('transform.edge_crease', 'E', 'PRESS', shift=True)
kmi = km.keymap_items.new('mesh.spin', 'R', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.fill', 'F', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.beautify_fill', 'F', 'PRESS', shift=True, alt=True)
kmi = km.keymap_items.new('mesh.quads_convert_to_tris', 'T', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'quad_method', 'BEAUTY')
kmi_props_setattr(kmi.properties, 'ngon_method', 'BEAUTY')
kmi = km.keymap_items.new('mesh.quads_convert_to_tris', 'T', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'quad_method', 'FIXED')
kmi_props_setattr(kmi.properties, 'ngon_method', 'CLIP')
kmi = km.keymap_items.new('mesh.tris_convert_to_quads', 'J', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.rip_move', 'V', 'PRESS')
kmi = km.keymap_items.new('mesh.rip_move_fill', 'V', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.rip_edge_move', 'D', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.merge', 'M', 'PRESS', alt=True)
kmi = km.keymap_items.new('transform.shrink_fatten', 'S', 'PRESS', alt=True)
kmi = km.keymap_items.new('mesh.edge_face_add', 'F', 'PRESS')
kmi = km.keymap_items.new('mesh.duplicate_move', 'D', 'PRESS', shift=True)
kmi = km.keymap_items.new('wm.call_menu', 'A', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'name', 'INFO_MT_mesh_add')
kmi = km.keymap_items.new('mesh.separate', 'P', 'PRESS')
kmi = km.keymap_items.new('mesh.split', 'Y', 'PRESS')
kmi = km.keymap_items.new('mesh.vert_connect', 'J', 'PRESS')
kmi = km.keymap_items.new('transform.vert_slide', 'V', 'PRESS', shift=True)
kmi = km.keymap_items.new('mesh.dupli_extrude_cursor', 'ACTIONMOUSE', 'CLICK', ctrl=True)
kmi_props_setattr(kmi.properties, 'rotate_source', True)
kmi = km.keymap_items.new('mesh.dupli_extrude_cursor', 'ACTIONMOUSE', 'CLICK', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'rotate_source', False)
kmi = km.keymap_items.new('wm.call_menu', 'X', 'PRESS')
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_delete')
kmi = km.keymap_items.new('wm.call_menu', 'DEL', 'PRESS')
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_delete')
kmi = km.keymap_items.new('mesh.dissolve_mode', 'X', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.dissolve_mode', 'DEL', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('mesh.knife_tool', 'K', 'PRESS')
kmi_props_setattr(kmi.properties, 'use_occlude_geometry', True)
kmi_props_setattr(kmi.properties, 'only_selected', False)
kmi = km.keymap_items.new('mesh.knife_tool', 'K', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'use_occlude_geometry', False)
kmi_props_setattr(kmi.properties, 'only_selected', True)
kmi = km.keymap_items.new('object.vertex_parent_set', 'P', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('wm.call_menu', 'W', 'PRESS')
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_specials')
kmi = km.keymap_items.new('wm.call_menu', 'F', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_faces')
kmi = km.keymap_items.new('wm.call_menu', 'E', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_edges')
kmi = km.keymap_items.new('wm.call_menu', 'V', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_edit_mesh_vertices')
kmi = km.keymap_items.new('wm.call_menu', 'H', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_hook')
kmi = km.keymap_items.new('wm.call_menu', 'U', 'PRESS')
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_uv_map')
kmi = km.keymap_items.new('wm.call_menu', 'G', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'VIEW3D_MT_vertex_group')
kmi = km.keymap_items.new('object.subdivision_set', 'ZERO', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 0)
kmi = km.keymap_items.new('object.subdivision_set', 'ONE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 1)
kmi = km.keymap_items.new('object.subdivision_set', 'TWO', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 2)
kmi = km.keymap_items.new('object.subdivision_set', 'THREE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 3)
kmi = km.keymap_items.new('object.subdivision_set', 'FOUR', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 4)
kmi = km.keymap_items.new('object.subdivision_set', 'FIVE', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'level', 5)
kmi = km.keymap_items.new('wm.context_cycle_enum', 'O', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.proportional_edit_falloff')
kmi = km.keymap_items.new('wm.context_toggle_enum', 'O', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.proportional_edit')
kmi_props_setattr(kmi.properties, 'value_1', 'DISABLED')
kmi_props_setattr(kmi.properties, 'value_2', 'ENABLED')
kmi = km.keymap_items.new('wm.context_toggle_enum', 'O', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.proportional_edit')
kmi_props_setattr(kmi.properties, 'value_1', 'DISABLED')
kmi_props_setattr(kmi.properties, 'value_2', 'CONNECTED')
kmi = km.keymap_items.new('wm.context_set_value', 'ONE', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.mesh_select_mode')
kmi_props_setattr(kmi.properties, 'value', '(True,False,False)')
kmi = km.keymap_items.new('wm.context_set_value', 'TWO', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.mesh_select_mode')
kmi_props_setattr(kmi.properties, 'value', '(False,True,False)')
kmi = km.keymap_items.new('wm.context_set_value', 'THREE', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.mesh_select_mode')
kmi_props_setattr(kmi.properties, 'value', '(False,False,True)')
kmi = km.keymap_items.new('mesh.bisect', 'B', 'PRESS', ctrl=True, alt=True)

# Map UV Editor
km = kc.keymaps.new('UV Editor', space_type='EMPTY', region_type='WINDOW', modal=False)

kmi = km.keymap_items.new('wm.context_toggle', 'Q', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.use_uv_sculpt')
kmi = km.keymap_items.new('uv.mark_seam', 'E', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.select', 'SELECTMOUSE', 'PRESS')
kmi_props_setattr(kmi.properties, 'extend', False)
kmi = km.keymap_items.new('uv.select', 'SELECTMOUSE', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'extend', True)
kmi = km.keymap_items.new('uv.select_loop', 'SELECTMOUSE', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi = km.keymap_items.new('uv.select_loop', 'SELECTMOUSE', 'PRESS', shift=True, alt=True)
kmi_props_setattr(kmi.properties, 'extend', True)
kmi = km.keymap_items.new('uv.select_split', 'Y', 'PRESS')
kmi = km.keymap_items.new('uv.select_border', 'B', 'PRESS')
kmi_props_setattr(kmi.properties, 'pinned', False)
kmi = km.keymap_items.new('uv.select_border', 'B', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'pinned', True)
kmi = km.keymap_items.new('uv.circle_select', 'C', 'PRESS')
kmi = km.keymap_items.new('uv.select_lasso', 'EVT_TWEAK_A', 'ANY', ctrl=True)
kmi_props_setattr(kmi.properties, 'deselect', False)
kmi = km.keymap_items.new('uv.select_lasso', 'EVT_TWEAK_A', 'ANY', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'deselect', True)
kmi = km.keymap_items.new('uv.select_linked', 'L', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'extend', False)
kmi = km.keymap_items.new('uv.select_linked_pick', 'L', 'PRESS')
kmi_props_setattr(kmi.properties, 'extend', False)
kmi = km.keymap_items.new('uv.select_linked', 'L', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'extend', True)
kmi = km.keymap_items.new('uv.select_linked_pick', 'L', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'extend', True)
kmi = km.keymap_items.new('uv.select_more', 'NUMPAD_PLUS', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.select_less', 'NUMPAD_MINUS', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.select_all', 'A', 'PRESS')
kmi_props_setattr(kmi.properties, 'action', 'TOGGLE')
kmi = km.keymap_items.new('uv.select_all', 'I', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'action', 'INVERT')
kmi = km.keymap_items.new('uv.select_pinned', 'P', 'PRESS', shift=True)
kmi = km.keymap_items.new('wm.call_menu', 'W', 'PRESS')
kmi_props_setattr(kmi.properties, 'name', 'IMAGE_MT_uvs_weldalign')
kmi = km.keymap_items.new('uv.stitch', 'V', 'PRESS')
kmi = km.keymap_items.new('uv.pin', 'P', 'PRESS')
kmi_props_setattr(kmi.properties, 'clear', False)
kmi = km.keymap_items.new('uv.pin', 'P', 'PRESS', alt=True)
kmi_props_setattr(kmi.properties, 'clear', True)
kmi = km.keymap_items.new('uv.unwrap', 'E', 'PRESS')
kmi = km.keymap_items.new('uv.minimize_stretch', 'V', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.pack_islands', 'P', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.average_islands_scale', 'A', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('uv.hide', 'H', 'PRESS')
kmi_props_setattr(kmi.properties, 'unselected', False)
kmi = km.keymap_items.new('uv.hide', 'H', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'unselected', True)
kmi = km.keymap_items.new('uv.reveal', 'H', 'PRESS', alt=True)
kmi = km.keymap_items.new('uv.cursor_set', 'ACTIONMOUSE', 'PRESS')
kmi = km.keymap_items.new('uv.tile_set', 'ACTIONMOUSE', 'PRESS', shift=True)
kmi = km.keymap_items.new('wm.call_menu', 'S', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'name', 'IMAGE_MT_uvs_snap')
kmi = km.keymap_items.new('wm.call_menu', 'TAB', 'PRESS', ctrl=True)
kmi_props_setattr(kmi.properties, 'name', 'IMAGE_MT_uvs_select_mode')
kmi = km.keymap_items.new('wm.context_cycle_enum', 'O', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.proportional_edit_falloff')
kmi = km.keymap_items.new('wm.context_toggle_enum', 'O', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.proportional_edit')
kmi_props_setattr(kmi.properties, 'value_1', 'DISABLED')
kmi_props_setattr(kmi.properties, 'value_2', 'ENABLED')
kmi = km.keymap_items.new('transform.translate', 'G', 'PRESS')
kmi = km.keymap_items.new('transform.translate', 'EVT_TWEAK_S', 'ANY')
kmi = km.keymap_items.new('transform.rotate', 'R', 'PRESS')
kmi = km.keymap_items.new('transform.resize', 'S', 'PRESS')
kmi = km.keymap_items.new('transform.shear', 'S', 'PRESS', shift=True, ctrl=True, alt=True)
kmi = km.keymap_items.new('transform.mirror', 'M', 'PRESS', ctrl=True)
kmi = km.keymap_items.new('wm.context_toggle', 'TAB', 'PRESS', shift=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.use_snap')
kmi = km.keymap_items.new('wm.context_menu_enum', 'TAB', 'PRESS', shift=True, ctrl=True)
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.snap_uv_element')
kmi = km.keymap_items.new('wm.context_set_string', 'ONE', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.uv_select_mode')
kmi_props_setattr(kmi.properties, 'value', 'VERTEX')
kmi = km.keymap_items.new('wm.context_set_string', 'TWO', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.uv_select_mode')
kmi_props_setattr(kmi.properties, 'value', 'EDGE')
kmi = km.keymap_items.new('wm.context_set_string', 'THREE', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.uv_select_mode')
kmi_props_setattr(kmi.properties, 'value', 'FACE')
kmi = km.keymap_items.new('wm.context_set_string', 'FOUR', 'PRESS')
kmi_props_setattr(kmi.properties, 'data_path', 'tool_settings.uv_select_mode')
kmi_props_setattr(kmi.properties, 'value', 'ISLAND')

